using System.Threading.Tasks;
using Xavier.Domain.Core.MessageBus.Bus;
using Xavier.Microservice.Domain.Events;
using Xavier.Microservices.Repository.Service.Interface;

namespace Xavier.Microservice.Domain.EventHandler
{
    public class NotificationEventHandler : IEventHandler<NotificationCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        public NotificationEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public Task Handle(NotificationCreatedEvent @event)
        {
            return Task.CompletedTask;
        }

       
    }
}
