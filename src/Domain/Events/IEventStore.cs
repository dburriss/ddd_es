using System;
using System.Collections.Generic;

namespace Domain.Events
{
    public interface IEventStore
    {
        IEnumerable<string> Tags { get; }
        IEnumerable<Guid> AggregateList(string tag);
        void Store(string tag, Guid aggregateId, long version, IEnumerable<IEvent> events);
        IEventStream Load(Guid aggregateId);
        void Register(IPostStoreHook hook);
        void Unregister(IPostStoreHook hook);
    }
}
