using System;

namespace Domain.Commands
{
    public class NewCommitteeCommand : ICommand, ITargetAggregate<Department>
    {
        private Guid _departmentId;
        public Guid CommitteeId { get; private set; }
        public string Name { get; private set; }
        public string Mandate { get; set; }

        public Guid AggregateId
        {
            get
            {
                return _departmentId;
            }
        }

        public NewCommitteeCommand(Guid committee, string name, string mandate, Guid departmentId)
        {
            _departmentId = departmentId;
            CommitteeId = committee;
            Name = name;
            Mandate = mandate;
        }

        protected NewCommitteeCommand()
        {}
    }
}