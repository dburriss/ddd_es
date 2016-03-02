using System;

namespace Domain.Events
{
    public class UpdateCommitteeEvent : IEvent
    {
        public Guid CommitteeId { get; protected set; }
        public Guid DepartmentId { get; protected set; }
        public string Name { get; protected set; }
        public string Mandate { get; set; }

        public UpdateCommitteeEvent(Guid committeeId, Guid departmentId, string name, string mandate)
        {
            CommitteeId = committeeId;
            DepartmentId = departmentId;
            Name = name;
            Mandate = mandate;
        }

        //just for serialization
        protected UpdateCommitteeEvent()
        {}
    }
}
