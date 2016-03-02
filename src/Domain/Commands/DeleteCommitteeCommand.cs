using System;

namespace Domain.Commands
{
    public class DeleteCommitteeCommand : ICommand, ITargetAggregate<Department>
    {
        private Guid _departmentId;
        public Guid CommitteeId { get; set; }

        public DeleteCommitteeCommand(Guid committeeId, Guid departmentId)
        {
            CommitteeId = committeeId;
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
