using System;
using Xavier.Domain.Core.MessageBus.Events;

namespace Xavier.Domain.Core.MessageBus.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
