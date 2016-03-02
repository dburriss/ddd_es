using System;

namespace Domain.Events
{
    public class NewCommitteeEvent : IEvent
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Mandate { get; set; }

        public NewCommitteeEvent(Guid committeeId, string name, string mandate)
        {
            Id = committeeId;
            Name = name;
            Mandate = mandate;
        }
    }
}
