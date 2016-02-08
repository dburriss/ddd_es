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
        public InMemoryEventStore()
        {
            streams = new  ConcurrentDictionary<Guid, IEventStream>();
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
        }
    }
}
