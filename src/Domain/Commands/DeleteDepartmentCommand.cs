using System;

namespace Domain.Commands
{
    public class DeleteDepartmentCommand : ICommand, ITargetAggregate<Department>
    {
        private Guid _departmentId;

        public DeleteDepartmentCommand(Guid departmentId)
        {
            _departmentId = departmentId;
        }

        public Guid AggregateId
        {
            get
            {
                return _departmentId;
            }
        }
    }
}
