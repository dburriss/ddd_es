using Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    
    public class InMemoryEventStore : IEventStore
    {
        private readonly IDictionary<Guid, IEventStream> streams;
        private readonly IDictionary<string, IPostStoreHook> postHooks;
        public InMemoryEventStore()
        {
            streams = new  ConcurrentDictionary<Guid, IEventStream>();
            postHooks = new  ConcurrentDictionary<string, IPostStoreHook>();
        }

        public IEventStream Load(Guid aggregateId)
        {
            IEventStream eventStream = null;
            
            if (!streams.TryGetValue(aggregateId, out eventStream))
            {
                eventStream = new InMemoryEventStream();
                streams[aggregateId] = eventStream;
            }
            return eventStream;
        }

        public void Store(Guid aggregateId, long version, IEnumerable<IEvent> events)
        {
            IEventStream stream = Load(aggregateId);
            if (stream.Version != version)
            {
                throw new InvalidOperationException("Stream has already been modified");
            }
            streams[aggregateId] = stream.Append(events);
            foreach (var key in postHooks.Keys)
            {
                postHooks[key].Handle(events);
            }
        }

        public void Register(IPostStoreHook hook)
        {
            postHooks[hook.Name] = hook;
        }

        public void Unregister(IPostStoreHook hook)
        {
            postHooks.Remove(hook.Name);
        }
    }
}
