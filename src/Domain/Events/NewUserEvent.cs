using System;

namespace Domain.Events
{
    public class NewUserEvent : IEvent
    {
        public Guid Id { get; protected set; }
        public string Username { get; protected set; }

        public NewUserEvent(Guid userId, string username)
        {
            Id = userId;
            Username = username;
        }
    }
}
