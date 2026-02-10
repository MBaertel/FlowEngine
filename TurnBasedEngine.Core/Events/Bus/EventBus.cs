using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.EventHandlers;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.Events.Bus
{
    public class EventBus : IEventBus
    {
        private readonly List<IEventHandler> _syncHandlers = new();
        private readonly List<IAsyncEventHandler> _asyncHandlers = new();
        private readonly Dictionary<Guid, IEventHandler> _handlerIds = new();

        public void Publish(IEvent evt)
        {
            foreach (var handler in _syncHandlers)
            {
                if(handler.CanHandle(evt))
                    handler.Handle(evt);
            }
        }

        public async Task PublishAsync(IEvent evt, CancellationToken cancellationToken = default)
        {
            foreach(var handler in _asyncHandlers)
            {
                if(handler.CanHandle(evt))
                    await handler.HandleAsync(evt, cancellationToken);
            }
        }

        public Guid Subscribe<TEvent>(IEventHandler handler) 
            where TEvent : IEvent
        {
            if(!_syncHandlers.Contains(handler))
                _syncHandlers.Add(handler);

            var newId = Guid.NewGuid();
            _handlerIds.Add(newId, handler);
            return newId;
        }

        public Guid Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return Subscribe<TEvent>(handler as IEventHandler);
        }

        public Guid SubscribeAsync<TEvent>(IAsyncEventHandler handler) where TEvent : IEvent
        {
            if (!_asyncHandlers.Contains(handler))
                _asyncHandlers.Add(handler);

            var newId = Guid.NewGuid();
            _handlerIds.Add(newId, handler);
            return newId;
        }

        public Guid SubscribeAsync<TEvent>(IAsyncEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return Subscribe<TEvent>(handler as IAsyncEventHandler);
        }

        public void Unsubscribe(IEventHandler handler)
        {
            if(handler is IAsyncEventHandler asyncHandler)
                _asyncHandlers.Remove(asyncHandler);
            else
                _syncHandlers.Remove(handler);

            var handlerId = _handlerIds.Where(x => x.Value == handler).First().Key;
            _handlerIds.Remove(handlerId);
        }

        public void Unsubscribe(Guid subId)
        {
            var handler = _handlerIds[subId];
            Unsubscribe(handler);
        }
    }
}
