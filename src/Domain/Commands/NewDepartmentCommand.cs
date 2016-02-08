using System;

namespace Domain.Commands
{
    public class NewDepartmentCommand : ICommand, ITargetAggregate<Department>
    {
        private Guid _departmentId;
        public string Name { get; set; }

        public NewDepartmentCommand(Guid departmentId, string name)
        {
            _departmentId = departmentId;
            Name = name;
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
