using System;
using Domain.Events;

namespace Domain
{
    public class Committee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mandate { get; set; }
        public bool IsActive { get; set; }

    }
}