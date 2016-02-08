using Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestInfrastructure
{
    
    public class InMemoryEventStore
    {
        private readonly IDictionary<Guid, IEventStream> eventStream;
        public InMemoryEventStore()
        {
            eventStream = new  ConcurrentDictionary<Guid, IEventStream>();
        }

        public IEventStream Load(Guid aggregateId)
        {
            return null;

            //InMemoryEventStream eventStream = streams.get(aggregateId);
            //if (eventStream == null)
            //{
            //    eventStream = new InMemoryEventStream();
            //    streams.put(aggregateId, eventStream);
            //}
            //return eventStream;
        }

        public void Store(Guid aggregateId, long version, IEnumerable<IEvent> events)
        {
            //InMemoryEventStream stream = loadEventStream(aggregateId);
            //if (stream.version() != version)
            //{
            //    throw new ConcurrentModificationException("Stream has already been modified");
            //}
            //streams.put(aggregateId, stream.append(events));
        }
    }
}
