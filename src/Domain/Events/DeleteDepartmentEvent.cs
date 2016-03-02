using System;

namespace Domain.Events
{
    public class DeleteDepartmentEvent : IEvent
    {
        public Guid DepartmentId { get; protected set; }

        public DeleteDepartmentEvent(Guid departmentId)
        {
            DepartmentId = departmentId;
        }

        //just for serialization
        protected DeleteDepartmentEvent()
        {}
    }
}
