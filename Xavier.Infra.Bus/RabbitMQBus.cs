using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xavier.Domain.Core.MessageBus.Bus;
using Xavier.Domain.Core.MessageBus.Commands;
using Xavier.Domain.Core.MessageBus.Events;

namespace Xavier.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;

        public RabbitMQBus(IMediator mediator)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }
        /// <summary>
        /// Send command for any object type
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        /// <summary>
        /// Allows for publishing of event for any type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string eventName = @event.GetType().Name;
                    channel.QueueDeclare(eventName, false, false, false, null);

                    string message = JsonConvert.SerializeObject(@event);

                    var messageBytes = Encoding.UTF8.GetBytes(message);

                    //publish
                    channel.BasicPublish("", eventName, null, messageBytes);
                }
            }
        }

        /// <summary>
        /// Allows for certain services to subscribe to events
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Subscribe<T, TH>() where T : Event 
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            //Check if event type doesn't already exists in the type of events
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
            
            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException($"Handler type {handlerType.Name} already is registered for" +
                                            $"{nameof(handlerType)}");
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory() {HostName = "localhost", DispatchConsumersAsync = true};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string eventName = typeof(T).Name;

                    channel.QueueDeclare(eventName, false, false, false, null);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += Consumer_Received;
                    
                    //consume
                    channel.BasicConsume(eventName, true, consumer);
                }
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body);

            await ProcessEvent(eventName, message);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                var subscriptions = _handlers[eventName];
                foreach (var subscription in subscriptions)
                {
                    var handler = Activator.CreateInstance(subscription);
                    if (handler == null) continue;

                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType);

                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                    await (Task) concreteType.GetMethod("Handle").Invoke(handler, new object[] {@event});

                }
            }
        }
    }
}
