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
        public ICollection<Committee> Committees { get; set; }
        public bool IsActive { get; set; }

        public Department()
        {
            Committees = new List<Committee>();
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
                throw new ArgumentException(nameof(cmd.AggregateId));

            var events = new List<IEvent>();
            events.Add(new NewCommitteeEvent(cmd.CommitteeId, cmd.Name));
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
                Name = @event.Name
            };

            Committees.Add(committee);
        }

        public void Process(IEvent @event)
        {
            var eventType = @event.GetType();
            var methodInfo = this.GetType().GetMethods().Where(m => m.Name == "Process" && m.GetParameters()[0].ParameterType == eventType).SingleOrDefault();
            if (methodInfo != null)
            {
                methodInfo.Invoke(this, new[] { @event });
            }
        }
    }
}
