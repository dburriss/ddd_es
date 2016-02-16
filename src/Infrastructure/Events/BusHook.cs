using Domain.Events;
using Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public class BusHook : IPostStoreHook
    {
        private readonly IBus _bus;
        private readonly string _name;

        public BusHook(string name, IBus bus)
        {
            _name = name;
            _bus = bus;
        }
        public string Name { get { return _name; } }

        public void Handle(IEnumerable<IEvent> events)
        {
            foreach (var ev in events)
            {
                _bus.Publish(ev);
            }
        }
    }
}
