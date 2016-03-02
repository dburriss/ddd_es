using Domain.Commands;
using Domain.Events;
using System.Collections.Generic;

namespace Domain
{
    public interface IAmAggregate
    {
        //IEnumerable<IEvent> Handle<TCommand>(TCommand command) where TCommand : ICommand;
        //void Process<TEvent>(TEvent @event) where TEvent : IEvent;
        void Process(IEvent @event);
    }
}
