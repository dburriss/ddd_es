using Domain.Commands;
using Domain.Events;
using System;
using System.Linq;
using ChimpLab.PhilosophicalMonkey;
using System.Collections.Generic;

namespace Domain
{
    public class ApplicationCommandService : IApplicationCommandService
    {
        private readonly IEventStore _eventStore;

        public ApplicationCommandService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public void Handle(ICommand command)
        {
            IEventStream eventStream = _eventStore.Load(command.AggregateId);
            Type type = GetTargetType(command);
            IAmAggregate root = CreateAggregate(type);

            if (root == null)
                throw new InvalidOperationException("Could not get instance for aggreagate from command.");

            foreach (var @event in eventStream)
            {
                Process(root, @event);
            }

            var events = Handle(root, command);
            if (events != null && events.Any())
            {
                var tag = EventTags.GetOrCreateIfNotExists(type);
                _eventStore.Store(tag, command.AggregateId, eventStream.Version, events);
            }
        }

        private IEnumerable<IEvent> Handle(IAmAggregate root, ICommand command)
        {
            var type = root.GetType();
            var paramType = command.GetType();
            var methodInfo = type.GetMethods().Where(m => m.Name == "Handle" && m.GetParameters()[0].ParameterType == paramType).SingleOrDefault();
            if (methodInfo != null)
            {
                return methodInfo.Invoke(root, new[] { command }) as IEnumerable<IEvent>;
            }
            throw new InvalidOperationException("No 'Handle' method on aggregate.");
        }

        private void Process(IAmAggregate root, IEvent @event)
        {
            var type = root.GetType();
            var paramType = @event.GetType();
            var methodInfo = type.GetMethods().Where(m => m.Name == "Process" && m.GetParameters()[0].ParameterType == paramType).SingleOrDefault();
            if (methodInfo != null)
            {
                methodInfo.Invoke(root, new[] { @event });
            }
            throw new InvalidOperationException("No 'Process' method on aggregate.");
        }

        private IAmAggregate CreateAggregate(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var instance = Activator.CreateInstance(type);
            return instance as IAmAggregate;

        }

        private Type GetTargetType(ICommand command)
        {
            var interfaces = command.GetType().GetInterfaces();
            var target = interfaces.Where(i => Reflect.IsGenericType(i) && i.GetGenericTypeDefinition() == typeof(ITargetAggregate<>)).SingleOrDefault();
            if (target != null)
            {
                var type = target.GetGenericArguments()[0];
                return type;
            }
            return null;
        }
    }
}
