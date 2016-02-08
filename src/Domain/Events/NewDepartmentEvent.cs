using System;

namespace Domain.Events
{
    public class NewDepartmentEvent : IEvent
    {
        public Guid DepartmentId { get; protected set; }
        public string Name { get; protected set; }

        public NewDepartmentEvent(Guid departmentId, string name)
        {
            DepartmentId = departmentId;
            Name = name;
        }

        //just for serialization
        protected NewDepartmentEvent()
        {}
    }
}
