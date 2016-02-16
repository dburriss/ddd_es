using Domain.Events;
using Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InfrastructureTests
{
    public class BusTests
    {
        [Fact]
        public async Task Publish_ToUnsubscribedEvent_NothingHappens()
        {
            var ev = new NewCommitteeEvent(Guid.NewGuid(), "Test Committee");
            bool hasRun = false;
            Action<object, IDictionary<string, string>> action = (o, h) => hasRun = true;

            using (var bus = new InMemoryBus())
            {
                await bus.Subscribe<NewDepartmentEvent>(action);
                await bus.Publish(ev);
                await bus.Start();
                Thread.Sleep(20);
            }
            
            Assert.False(hasRun);
        }

        [Fact]
        public async Task Publish_ToSubscribedEvent_RunsTask()
        {
            var ev = new NewCommitteeEvent(Guid.NewGuid(), "Test Committee");
            bool hasRun = false;
            Action<object, IDictionary<string, string>> action = (o, h) => hasRun = true;

            using (var bus = new InMemoryBus())
            {
                await bus.Subscribe<NewCommitteeEvent>(action);
                await bus.Publish(ev);
                await bus.Start();
                Thread.Sleep(20);//give task time to complete
            }
            
            Assert.True(hasRun);
        }
    }
}
