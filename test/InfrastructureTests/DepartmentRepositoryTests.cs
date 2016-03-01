using Domain;
using Domain.Events;
using Infrastructure;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InfrastructureTests
{
    public class DepartmentRepositoryTests
    {
        string departmentTag = typeof(Department).AssemblyQualifiedName;

        [Fact]
        public void GetById_WithEmptyEventStore_ReturnsNull()
        {
            var id = Guid.NewGuid();
            IDepartmentQueryRepository sut = GetSut();
            var department = sut.GetById(id);
            Assert.Null(department.Name);
        }

        [Fact]
        public void GetById_WithNewDepartment_ReturnsPopulatedDepartment()
        {
            var id = Guid.NewGuid();
            var committeeId = Guid.NewGuid();
            var streamData = new List<Tuple<string, Guid, long, IEnumerable<IEvent>>>
            {
                new Tuple<string, Guid, long, IEnumerable<IEvent>>(departmentTag, id, 0, new List<IEvent>
                {
                    new NewDepartmentEvent(id, "Test Department"),
                    new NewCommitteeEvent(committeeId, "Test Committee")
                })
            };
            IDepartmentQueryRepository sut = GetSut(streamData);
            var department = sut.GetById(id);
            Assert.Equal("Test Department", department.Name);
            Assert.NotEmpty(department.Committees);
        }


        [Fact]
        public void Query_EmptyStore_Returns0Departments()
        {
            IDepartmentQueryRepository sut = GetSut();
            var departments = sut.Query();
            Assert.True(departments.Count() == 0);
        }

        [Fact]
        public void Query_WithNoFilterOn2Departments_Returns2Departments()
        {
            var id1 = Guid.NewGuid();
            var committeeId1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var committeeId2 = Guid.NewGuid();
            var streamData = new List<Tuple<string, Guid, long, IEnumerable<IEvent>>>
            {
                new Tuple<string, Guid, long, IEnumerable<IEvent>>(departmentTag, id1, 0, new List<IEvent>
                {
                    new NewDepartmentEvent(id1, "Test Department 1"),
                    new NewCommitteeEvent(committeeId1, "Test Committee 1")
                }),
                new Tuple<string, Guid, long, IEnumerable<IEvent>>(departmentTag, id2, 0, new List<IEvent>
                {
                    new NewDepartmentEvent(id2, "Test Department 2"),
                    new NewCommitteeEvent(committeeId2, "Test Committee 2")
                })
            };
            IDepartmentQueryRepository sut = GetSut(streamData);
            var departments = sut.Query();
            Assert.True(departments.Count() == 2);
        }

        [Fact]
        public void Query_WithFilterOnId_Returns1Departments()
        {
            var id1 = Guid.NewGuid();
            var committeeId1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var committeeId2 = Guid.NewGuid();
            var streamData = new List<Tuple<string, Guid, long, IEnumerable<IEvent>>>
            {
                new Tuple<string, Guid, long, IEnumerable<IEvent>>(departmentTag, id1, 0, new List<IEvent>
                {
                    new NewDepartmentEvent(id1, "Test Department 1"),
                    new NewCommitteeEvent(committeeId1, "Test Committee 1")
                }),
                new Tuple<string, Guid, long, IEnumerable<IEvent>>(departmentTag, id2, 0, new List<IEvent>
                {
                    new NewDepartmentEvent(id2, "Test Department 2"),
                    new NewCommitteeEvent(committeeId2, "Test Committee 2")
                })
            };
            IDepartmentQueryRepository sut = GetSut(streamData);
            var departments = sut.Query(x => x.Id == id2);
            Assert.True(departments.Count() == 1);
            Assert.Equal("Test Department 2", departments.First().Name);
        }

        private IDepartmentQueryRepository GetSut(IEnumerable<Tuple<string, Guid, long, IEnumerable<IEvent>>> events = null)
        {
            if (events == null)
                events = new List<Tuple<string, Guid, long, IEnumerable<IEvent>>>();

            var es = new InMemoryEventStore(events);
            var repo = new InMemoryCachedDepartmentRepository(es);
            return repo;
        }
    }
}
