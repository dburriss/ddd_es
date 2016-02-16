using System;
using System.Collections.Generic;

namespace Domain.Events
{
    public interface IEventStore
    {
        void Store(Guid aggregateId, long version, IEnumerable<IEvent> events);
        IEventStream Load(Guid aggregateId);
        void Register(IPostStoreHook hook);
        void Unregister(IPostStoreHook hook);
    }
}
