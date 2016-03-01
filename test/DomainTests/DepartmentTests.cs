using Domain;
using Domain.Commands;
using Domain.Events;
using System;
using System.Linq;
using Xunit;

namespace DomainTests
{
    public class DepartmentTests
    {
        const string departmentId = "0630b45e-a294-44d4-8324-6c25d05e89aa";
        [Fact]
        public void Handle_NewCommitteeCommand_RetursAtLeast1Event()
        {
            var department = GetTestDepartment();
            var cmd = new NewCommitteeCommand(Guid.NewGuid(), "Test Committee 1", department.Id);
            var events = department.Handle(cmd);

            Assert.NotEmpty(events);
        }

        [Fact]
        public void Handle_NewCommitteeCommand_RetursAtNewCommitteeEvent()
        {
            var department = GetTestDepartment();
            var cmd = new NewCommitteeCommand(Guid.NewGuid(), "Test Committee 1", department.Id);
            var events = department.Handle(cmd);

            Assert.True(events.Any(x => x.GetType() == typeof(NewCommitteeEvent)));
        }

        [Fact]
        public void Process_NewDepartmentEvent_SetsNAmeAndId()
        {
            var id = Guid.NewGuid();
            var @event = new NewDepartmentEvent(id, "Test Department");
            var department = new Department();
            department.Process(@event);

            Assert.Equal(id, department.Id);
            Assert.Equal("Test Department", department.Name);
        }

        [Fact]
        public void Process_NewCommitteeEvent_SetsCommitteeProperties()
        {
            var id = Guid.NewGuid();
            var @event = new NewCommitteeEvent(id, "Test Committee");
            var department = new Department();
            department.Process(@event);

            var committee = department.Committees.First();
            Assert.Equal(id, committee.Id);
            Assert.Equal("Test Committee", committee.Name);
        }

        private Department GetTestDepartment()
        {
            var id = Guid.Parse(departmentId);
            var department = new Department()
            {
                Id = id,
                Name = "Test Department"
            };

            return department;
        }
    }
}
