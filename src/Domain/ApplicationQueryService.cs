using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class ApplicationQueryService
    {
        private readonly IEventStore _eventStore;

        public ApplicationQueryService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public IEnumerable<Committee> GetCommittees()
        {
            return null;
        }
    }
}
