using System.Linq;
using Xavier.Domain.Core.MessageBus.Bus;
using Xavier.Microservices.Repository.Entities;
using Xavier.Microservices.Repository.Service.Interface;

namespace Xavier.Microservices.Repository.Service.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationMicroserviceContext _context;
        private readonly IEventBus _eventBus;

        public NotificationService(NotificationMicroserviceContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }


        /// <summary>
        /// Service to create record
        /// </summary>
        /// <param name="model"></param>
        public void Create(Notification model)
        {
            _context.Notifications.Add(model);
            _context.SaveChanges();
        }

        /// <summary>
        /// Service to get all notifications fetched from the bus and stored in the db
        /// </summary>
        /// <returns></returns>
        public IQueryable<Notification> GetAllNotifications()
        {
            return _context.Notifications.AsQueryable();
        }
    }
}