using System.Collections.Generic;

namespace Domain.Events
{
    public interface IEventStream
    {
        long Version { get; }
        IEnumerable<IEvent> Events { get; }
        IEventStream Append(IEnumerable<IEvent> events);
    }
}
