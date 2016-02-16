using System.Collections.Generic;

namespace Domain.Events
{
    public interface IPostStoreHook
    {
        string Name { get; }
        void Handle(IEnumerable<IEvent> events);
    }
}
