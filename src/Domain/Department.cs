using Domain.Commands;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Department : IAmAggregate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        private ICollection<Committee> committees { get; set; }
        public IEnumerable<Committee> Committees
        {
            get
            {
                return committees.Where(x => x.IsActive);
            }
        }
        public bool IsActive { get; set; }

        public Department()
        {
            committees = new List<Committee>();
        }

        public IEnumerable<IEvent> Handle(NewDepartmentCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            if (cmd.AggregateId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.AggregateId));

            if (string.IsNullOrEmpty(nameof(cmd.Name)))
                throw new ArgumentException(nameof(cmd.AggregateId));

            var events = new List<IEvent>();
            events.Add(new NewDepartmentEvent(cmd.AggregateId, cmd.Name));
            return events;
        }

        public IEnumerable<IEvent> Handle(UpdateDepartmentCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            if (cmd.AggregateId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.AggregateId));

            if (string.IsNullOrEmpty(nameof(cmd.Name)))
                throw new ArgumentException(nameof(cmd.AggregateId));

            var events = new List<IEvent>();
            events.Add(new UpdateDepartmentEvent(cmd.AggregateId, cmd.Name));
            return events;
        }

        public IEnumerable<IEvent> Handle(DeleteDepartmentCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            if (cmd.AggregateId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.AggregateId));

            var events = new List<IEvent>();
            events.Add(new DeleteDepartmentEvent(cmd.AggregateId));
            return events;
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
                throw new ArgumentException(nameof(cmd.Name));
            if (string.IsNullOrEmpty(nameof(cmd.Mandate)))
                throw new ArgumentException(nameof(cmd.Mandate));

            var events = new List<IEvent>();
            events.Add(new NewCommitteeEvent(cmd.CommitteeId, cmd.Name, cmd.Mandate));
            return events;
        }

        public IEnumerable<IEvent> Handle(UpdateCommitteeCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));
            if (cmd.AggregateId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.AggregateId));
            if (cmd.CommitteeId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.CommitteeId));
            if (string.IsNullOrEmpty(nameof(cmd.Name)))
                throw new ArgumentException(nameof(cmd.Name));
            if (string.IsNullOrEmpty(nameof(cmd.Mandate)))
                throw new ArgumentException(nameof(cmd.Mandate));

            var events = new List<IEvent>();
            events.Add(new UpdateCommitteeEvent(cmd.CommitteeId, cmd.AggregateId, cmd.Name, cmd.Mandate));
            return events;
        }

        public IEnumerable<IEvent> Handle(DeleteCommitteeCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));
            if (cmd.AggregateId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.AggregateId));
            if (cmd.CommitteeId == Guid.Empty)
                throw new ArgumentException(nameof(cmd.CommitteeId));

            var events = new List<IEvent>();
            events.Add(new DeleteCommitteeEvent(cmd.CommitteeId));
            return events;
        }

        public void Process(NewDepartmentEvent @event)
        {
            this.Id = @event.DepartmentId;
            this.Name = @event.Name;
            this.IsActive = true;
        }

        public void Process(UpdateDepartmentEvent @event)
        {
            this.Name = @event.Name;
        }

        public void Process(DeleteDepartmentEvent @event)
        {
            this.IsActive = false;
        }

        public void Process(NewCommitteeEvent @event)
        {
            var committee = new Committee()
            {
                Id = @event.Id,
                Name = @event.Name,
                Mandate = @event.Mandate,
                IsActive = true
            };

            committees.Add(committee);
        }

        public void Process(UpdateCommitteeEvent @event)
        {
            var committee = committees.SingleOrDefault(x => x.Id == @event.CommitteeId);
            if (committee != null)
            {
                committee.Name = @event.Name;
                committee.Mandate = @event.Mandate;
            }
        }

        public void Process(DeleteCommitteeEvent @event)
        {
            var committee = committees.SingleOrDefault(x => x.Id == @event.CommitteeId);
            if (committee != null)
            {
                committee.IsActive = false;
            }
        }

        public void Process(IEvent @event)
        {
            var eventType = @event.GetType();
            var methodInfo = this.GetType().GetMethods().Where(m => m.Name == "Process" && m.GetParameters()[0].ParameterType == eventType).SingleOrDefault();
            if (methodInfo != null)
            {
                methodInfo.Invoke(this, new[] { @event });
                return;
            }
            throw new InvalidOperationException("No 'Process' method on aggregate for event: " + eventType.Name);
        }
    }
}
