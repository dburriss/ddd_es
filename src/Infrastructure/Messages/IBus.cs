using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public interface IBus : IDisposable
    {
        Task Subscribe<TEvent>(Action<object, IDictionary<string, string>> action);
        Task Subscribe(Type eventType, Action<object, IDictionary<string, string>> action);
        Task Unsubscribe<TEvent>();
        Task Unsubscribe(Type eventType);
        Task Publish(object message, Dictionary<string, string> optionalHeaders = null);
    }
}
