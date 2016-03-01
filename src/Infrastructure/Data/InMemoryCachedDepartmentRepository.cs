using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Domain.Events;

namespace Infrastructure.Data
{
    public class InMemoryCachedDepartmentRepository : IDepartmentQueryRepository
    {
        string tag;
        private readonly IEventStore _eventStore;
        public InMemoryCachedDepartmentRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
            tag = EventTags.GetOrCreateIfNotExists(typeof(Department));
        }
        public Department GetById(Guid id)
        {
            var stream = _eventStore.Load(id);
            var department = new Department();
            foreach (var ev in stream)
            {
                department.Process(ev);
            }
            return department;
        }

        public IEnumerable<Department> Query(Expression<Func<Department, bool>> filter = null)
        {
            if (filter == null)
            {
                filter = x => true;
            }
                
            var ids = _eventStore.AggregateList(tag);
            var departments = GetAllDepartments(ids);
            return departments.Where(filter?.Compile());
        }

        private IEnumerable<Department> GetAllDepartments(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                yield return GetById(id);
            }
        }
    }
}
