using System;
using Xavier.Domain.Core.MessageBus.Events;

namespace Xavier.Microservice.Domain.Events
{
    public class NotificationCreatedEvent : Event
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public Int64 TransactionAmount { get; private set; }

        public NotificationCreatedEvent(string from, string to, Int64 amount)
        {
            From = from;
            To = to;
            TransactionAmount = amount;
        }
    }
}
