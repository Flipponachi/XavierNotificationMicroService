using System.Threading.Tasks;
using Xavier.Domain.Core.MessageBus.Events;

namespace Xavier.Domain.Core.MessageBus.Bus
{
    public interface IEventHandler
    { }

    public interface IEventHandler<in TEvent> : IEventHandler 
        where TEvent : Event
    {
        Task Handle(TEvent @event);
    }
}