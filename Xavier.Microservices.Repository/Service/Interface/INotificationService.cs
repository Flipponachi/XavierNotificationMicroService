using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Microservices.Repository.Entities;

namespace Xavier.Microservices.Repository.Service.Interface
{
    public interface INotificationService
    {
        /// <summary>
        /// Service to create record
        /// </summary>
        /// <param name="model"></param>
        void Create(Notification model);

        /// <summary>
        /// Service to get all notifications fetched from the bus and stored in the db
        /// </summary>
        /// <returns></returns>
        IQueryable<Notification> GetAllNotifications();
    }
}
