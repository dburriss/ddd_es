using Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{

    public class InMemoryEventStore : IEventStore
    {
        private readonly IDictionary<string, List<Guid>> aggregateIndex;
        private readonly IDictionary<Guid, IEventStream> streams;
        private readonly IDictionary<string, IPostStoreHook> postHooks;

        public InMemoryEventStore()
        {
            aggregateIndex = new ConcurrentDictionary<string, List<Guid>>();
            streams = new ConcurrentDictionary<Guid, IEventStream>();
            postHooks = new ConcurrentDictionary<string, IPostStoreHook>();
        }

        public InMemoryEventStore(IEnumerable<Tuple<string, Guid, long, IEnumerable<IEvent>>> eventsInfo) : this()
        {
            foreach (var evInfo in eventsInfo)
            {
                var tag = evInfo.Item1;
                var aggregateId = evInfo.Item2;
                var version = evInfo.Item3;
                var events = evInfo.Item4;
                InternalStore(tag, aggregateId, version, events);
            }
        }

        public IEnumerable<string> Tags
        {
            get
            {
                return aggregateIndex.Keys;
            }
        }

        public IEnumerable<Guid> AggregateList(string tag)
        {
            if (aggregateIndex.ContainsKey(tag))
            {
                return aggregateIndex[tag];
            }
            return new List<Guid>();
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

        public void Store(string tag, Guid aggregateId, long version, IEnumerable<IEvent> events)
        {
            InternalStore(tag, aggregateId, version, events);

            ProcessPostHooks(events);
        }

        private void InternalStore(string tag, Guid aggregateId, long version, IEnumerable<IEvent> events)
        {
            IEventStream stream = Load(aggregateId);
            if (stream.Version != version)
            {
                throw new InvalidOperationException("Stream has already been modified");
            }

            streams[aggregateId] = stream.Append(events);
            UpdateAggregateIndex(tag, aggregateId);
        }

        private void UpdateAggregateIndex(string tag, Guid aggregateId)
        {
            if (!aggregateIndex.ContainsKey(tag))
            {
                aggregateIndex[tag] = new List<Guid> { aggregateId };
            }
            else if (!aggregateIndex[tag].Any(x => x == aggregateId))
            {
                aggregateIndex[tag].Add(aggregateId);
            }
        }

        private void ProcessPostHooks(IEnumerable<IEvent> events)
        {
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
