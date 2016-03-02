using Domain;
using Domain.Events;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InfrastructureTests
{
    public class EventStoreTests
    {
        string departmentTag = typeof(Department).AssemblyQualifiedName;
        string userTag = typeof(User).AssemblyQualifiedName;
        [Fact]
        public void Store_WithRegisteredHook_TriggersHook()
        {
            var id = Guid.NewGuid();
            var store = new InMemoryEventStore();
            var counterHook = new CounterHook("myCounter");
            store.Register(counterHook);
            store.Store(departmentTag, id, 0, new IEvent[] { new NewCommitteeEvent(id, "Test Committee", "mandate") });
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
            store.Store(departmentTag, id, 0, new IEvent[] { new NewCommitteeEvent(id, "Test Committee", "mandate") });
            Assert.Equal(0, counterHook.Count);
        }

        [Fact]
        public void Store_WithNoDepartment_AddsAggregateIdToIndex()
        {
            var id = Guid.NewGuid();
            var store = new InMemoryEventStore();
            Assert.Equal(0, store.Tags.Count());
            store.Store(departmentTag, id, 0, new IEvent[] { new NewDepartmentEvent(id, "Test Committee") });
            Assert.Equal(1, store.Tags.Count());
        }

        [Fact]
        public void Store_2Departments_Results1Tag2Ids()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var store = new InMemoryEventStore();
            Assert.Equal(0, store.Tags.Count());
            store.Store(departmentTag, id1, 0, new IEvent[] { new NewDepartmentEvent(id1, "Test Department 1") });
            store.Store(departmentTag, id2, 0, new IEvent[] { new NewDepartmentEvent(id2, "Test Department 2") });
            Assert.Equal(1, store.Tags.Count());
            Assert.Equal(2, store.AggregateList(departmentTag).Count());
        }

        [Fact]
        public void Store_1Department1User_Results2Tag1IdEach()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var store = new InMemoryEventStore();
            Assert.Equal(0, store.Tags.Count());
            store.Store(departmentTag, id1, 0, new IEvent[] { new NewDepartmentEvent(id1, "New Department 1"), new NewCommitteeEvent(id1, "Test Committee 1", "mandate") });
            store.Store(userTag, id2, 0, new IEvent[] { new NewUserEvent(id2, "Test User 1") });
            Assert.Equal(2, store.Tags.Count());
            Assert.Equal(1, store.AggregateList(departmentTag).Count());
            Assert.Equal(1, store.AggregateList(userTag).Count());
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
