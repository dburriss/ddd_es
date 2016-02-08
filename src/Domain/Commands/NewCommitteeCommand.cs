using System;

namespace Domain.Commands
{
    public class NewCommitteeCommand : ICommand
    {
        private Guid _departmentId;
        public Guid CommitteeId { get; private set; }
        public string Name { get; private set; }

        public Guid AggregateId
        {
            get
            {
                return _departmentId;
            }
        }

        public NewCommitteeCommand(Guid committee, string name, Guid departmentId)
        {
            _departmentId = departmentId;
            CommitteeId = committee;
            Name = name;
        }
    }
}