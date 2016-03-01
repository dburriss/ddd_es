using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain
{
    public interface IDepartmentQueryRepository
    {
        Department GetById(Guid id);
        IEnumerable<Department> Query(Expression<Func<Department, bool>> filter = null);
    }
}
