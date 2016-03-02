using System;

namespace Domain.Events
{
    public class UpdateDepartmentEvent : IEvent
    {
        public Guid DepartmentId { get; protected set; }
        public string Name { get; protected set; }

        public UpdateDepartmentEvent(Guid departmentId, string name)
        {
            DepartmentId = departmentId;
            Name = name;
        }

        //just for serialization
        protected UpdateDepartmentEvent()
        {}
    }
}
