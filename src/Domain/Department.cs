using Domain.Commands;
using Domain.Events;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Committee> Committees { get; set; }

        public Department()
        {
            Committees = new List<Committee>();
        }

        public IEnumerable<IEvent> Handle(NewCommitteeCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));
            if (cmd.AggregateId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.AggregateId));
            if (cmd.CommitteeId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.CommitteeId));
            if (string.IsNullOrEmpty(nameof(cmd.Name)))
                throw new ArgumentException(nameof(cmd.AggregateId));

            var events = new List<IEvent>();
            events.Add(new NewCommitteeEvent(cmd.CommitteeId, cmd.Name));
            return events;
        }

        public void Process(NewCommitteeEvent @event)
        {
            var committee = new Committee()
            {
                Id = @event.Id,
                Name = @event.Name
            };

            Committees.Add(committee);
        }
    }
}
