using System.Threading.Tasks;
using Xavier.Domain.Core.MessageBus.Commands;
using Xavier.Domain.Core.MessageBus.Events;

namespace Xavier.Domain.Core.MessageBus.Bus
{
    /// <summary>
    /// Contract for using an event bus
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Send command for any object type
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SendCommand<T>(T command) where T : Command;

        /// <summary>
        /// Allows for publishing of event for any type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        void Publish<T>(T @event) where T : Event;

        /// <summary>
        /// Allows for certain services to subscribe to events
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void Subscribe<T, TH>() 
            where T : Event 
            where TH : IEventHandler<T>;
    }
}
