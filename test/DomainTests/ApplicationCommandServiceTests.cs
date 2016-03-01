using Domain;
using Domain.Commands;
using Domain.Events;
using Infrastructure;
using System;
using System.Linq;
using Xunit;

namespace DomainTests
{
    public class ApplicationCommandServiceTests : IDisposable
    {
        private InMemoryEventStore store;

        public ApplicationCommandServiceTests()
        {
            store = new InMemoryEventStore();
        }

        public void Dispose()
        {
            store = null;
        }

        [Fact]
        public void Handle_NewDepartmentCommand_PersistsNewDepartmentEvent()
        {
            var sut = GetApplicationService();
            var departmentId = Guid.NewGuid();
            var cmd = new NewDepartmentCommand(departmentId, "Test Department 1");
            sut.Handle(cmd);
            Assert.True(store.Load(departmentId).Any(x => x.GetType() == typeof(NewDepartmentEvent)));
        }

        private ApplicationCommandService GetApplicationService()
        {
            
            var service = new ApplicationCommandService(store);
            return service;
        }


    }
}
