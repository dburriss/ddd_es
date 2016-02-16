using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messages
{
    public class InMemoryBus : IBus
    {
        bool isRunning = false;
        //[ThreadStatic] //so that each thread has its own callbacks
        private static Dictionary<Type, List<Action<object, IDictionary<string, string>>>> topics;
        //[ThreadStatic]
        private static Dictionary<Type, Action<object, IDictionary<string, string>>> queues;

        private List<Task> tasks;

        public InMemoryBus()
        {
            topics = new Dictionary<Type, List<Action<object, IDictionary<string, string>>>>();
            queues = new Dictionary<Type, Action<object, IDictionary<string, string>>>();
            tasks = new List<Task>();
        }

        bool _disposing;
        public void Dispose()
        {
            if (_disposing) return;

            try
            {
                _disposing = true;

                isRunning = false;
                topics.Clear();
                queues.Clear();
                tasks.Clear();
            }
            finally
            {
                _disposing = false;
            }
        }

        public async Task Publish(object message, Dictionary<string, string> optionalHeaders = null)
        {
            await Task.Run(() =>
            {
                var type = message.GetType();
                if (topics.ContainsKey(type))
                {
                    var actionList = topics[type];
                    tasks.Add(new Task(() => actionList.ForEach(a => a(message, optionalHeaders))));
                }
            });
        }

        public Task Send(object message, Dictionary<string, string> optionalHeaders = null)
        {
            throw new NotImplementedException();
        }

        public async Task Subscribe<TEvent>(Action<object, IDictionary<string, string>> action)
        {
            var type = typeof(TEvent);
            await Subscribe(type, action);
        }

        public Task Subscribe(Type eventType, Action<object, IDictionary<string, string>> action)
        {
            if (!topics.ContainsKey(eventType))
            {
                topics.Add(eventType, new List<Action<object, IDictionary<string, string>>>());
            }
            return Task.Run(() => topics[eventType].Add(action));
        }

        public async Task Unsubscribe(Type eventType)
        {
            await Task.Run(() =>
            {
                if (!topics.ContainsKey(eventType))
                {
                    tasks.Add(new Task(() => topics.Remove(eventType)));
                }
            });
        }

        public async Task Unsubscribe<TEvent>()
        {
            var type = typeof(TEvent);
            await Unsubscribe(type);
        }

        public async Task Start()
        {
            Task.Run(() =>
            {
                isRunning = true;
                while (isRunning)
                {
                    if (tasks.Any())
                    {
                        var task = tasks.First();
                        tasks.Remove(task);
                        task.Start();
                        Thread.Sleep(10);
                    }
                }
            });
        }
    }
}
