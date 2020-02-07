using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Xavier.Microservices.Repository.Entities;

namespace Xavier.Microservices.Repository
{
    public class NotificationMicroserviceContext : DbContext
    {
        public NotificationMicroserviceContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Notification> Notifications { get; set; }
    }
}
