using System;

namespace Domain.Commands
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}
