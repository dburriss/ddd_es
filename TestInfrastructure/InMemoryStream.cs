using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestInfrastructure
{
    public class InMemoryStream : IEventStream
    {
        private readonly long _version;
        private readonly List<IEvent> _events;

        public InMemoryStream()
        {
            _version = 0;
            _events = new List<IEvent>();
        }

        public InMemoryStream(long version, IEnumerable<IEvent> events)
        {
            _version = version;
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


        public IEventStream Append(IEnumerable<IEvent> events)
        {
            //List<IEvent> events = new List<IEvent>(this.events);
            //events.addAll(newEvents);
            //return new InMemoryEventStream(version + 1, Collections.unmodifiableList(events));
            return new InMemoryStream(_version, _events);
        }

    }
}
