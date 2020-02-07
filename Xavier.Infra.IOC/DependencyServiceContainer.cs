using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xavier.Domain.Core.MessageBus.Bus;
using Xavier.Infra.Bus;
using Xavier.Microservice.Domain.EventHandler;
using Xavier.Microservice.Domain.Events;
using Xavier.Microservices.Repository;
using Xavier.Microservices.Repository.Service.Implementation;
using Xavier.Microservices.Repository.Service.Interface;

namespace Xavier.Infra.IOC
{
    public class DependencyServiceContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Service bus
            services.AddTransient<IEventBus, RabbitMQBus>();

            //Data commands
            services.AddTransient<IEventHandler<NotificationCreatedEvent>, NotificationEventHandler>();

            //Data access related services
            services.AddTransient<INotificationService, NotificationService>();

            services.AddTransient<NotificationMicroserviceContext>();
        }
    }
}
