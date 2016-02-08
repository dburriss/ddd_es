using System;

namespace Domain.Events
{
    public class NewCommitteeEvent : IEvent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public NewCommitteeEvent(Guid committeeId, string name)
        {
            Id = committeeId;
            Name = name;
        }
    }
}
