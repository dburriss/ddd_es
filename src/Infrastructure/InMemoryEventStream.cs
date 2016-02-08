using Domain.Events;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Infrastructure
{
    public class InMemoryEventStream : IEventStream
    {
        private readonly long _version;
        private readonly List<IEvent> _events;

        public InMemoryEventStream()
        {
            _version = 0;
            _events = new List<IEvent>();
        }

        public InMemoryEventStream(long version, IEnumerable<IEvent> events)
        {
            _version = version;
            _events = new List<IEvent>();
            _events.AddRange(events);
        }

        public long Version
        {
            get
            {
                return _version;
            }
        }

        public IEnumerable<IEvent> Events
        {
            get
            {
                return _events;
            }
        }


        public IEventStream Append(IEnumerable<IEvent> newEvents)
        {
            List<IEvent> events = new List<IEvent>(_events);
            events.AddRange(newEvents);
            return new InMemoryEventStream(Version + 1, events);//events should be immutable
            //return new InMemoryEventStream(_version, _events);
        }

        public IEnumerator<IEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _events.GetEnumerator();
        }
    }
}
