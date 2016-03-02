using System;

namespace Domain.Commands
{
    public class UpdateCommitteeCommand : ICommand, ITargetAggregate<Department>
    {
        private Guid _departmentId;
        public Guid CommitteeId { get; set; }
        public string Name { get; set; }
        public string Mandate { get; set; }

        public UpdateCommitteeCommand(Guid committeeId, Guid departmentId, string name, string mandate)
        {
            CommitteeId = committeeId;
            _departmentId = departmentId;
            Name = name;
            Mandate = mandate;
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
