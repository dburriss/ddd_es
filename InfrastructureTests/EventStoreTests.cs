using Domain.Events;
using Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace InfrastructureTests
{
    public class EventStoreTests
    {
        [Fact]
        public void Store_WithRegisteredHook_TriggersHook()
        {
            var id = Guid.NewGuid();
            var store = new InMemoryEventStore();
            var counterHook = new CounterHook("myCounter");
            store.Register(counterHook);
            store.Store(id, 0, new IEvent[] { new NewCommitteeEvent(Guid.NewGuid(), "Test Committee") });
            Assert.Equal(1, counterHook.Count);
        }

        [Fact]
        public void Store_WithUnregisteredHook_DoesNotTriggerHook()
        {
            var id = Guid.NewGuid();
            var store = new InMemoryEventStore();
            var counterHook = new CounterHook("myCounter");
            store.Register(counterHook);
            store.Unregister(counterHook);
            store.Store(id, 0, new IEvent[] { new NewCommitteeEvent(Guid.NewGuid(), "Test Committee") });
            Assert.Equal(0, counterHook.Count);
        }
    }

    internal class CounterHook : IPostStoreHook
    {
        private readonly string _name;
        public int Count { get; set; }
        public CounterHook(string uniqueName)
        {
            _name = uniqueName;
            Count = 0;
        }
        public string Name { get { return _name; } }

        public void Handle(IEnumerable<IEvent> events)
        {
            foreach (var e in events)
            {
                Count++;
            }
        }
    }
}
