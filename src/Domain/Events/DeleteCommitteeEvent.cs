using System;

namespace Domain.Events
{
    public class DeleteCommitteeEvent : IEvent
    {
        public Guid CommitteeId { get; protected set; }

        public DeleteCommitteeEvent(Guid committeeId)
        {
            CommitteeId = committeeId;
        }

        //just for serialization
        protected DeleteCommitteeEvent()
        {}
    }
}
