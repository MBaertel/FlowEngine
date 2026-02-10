using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using FlowEngine.Core.EventHandlers;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.Events.Bus
{
    /// <summary>
    /// Helper Methods collected here to avoid polluting the IEventBus interface.
    /// </summary>
    public static class EventBusExtensions
    {
        /// <summary>
        /// Subscribe Passthrough method to subscribe directly with a lambda.
        /// </summary>
        public static Guid Subscribe<TEvent>(this IEventBus eventBus,Action<TEvent> lambda)
            where TEvent : IEvent
        {
            var handler = new LambdaEventHandler<TEvent>(lambda);
            return eventBus.Subscribe(handler);
        }

        /// <summary>
        /// SubscribeAsync Passthrough method to subscribe directly with a lambda.
        /// </summary>
        public static Guid SubscribeAsync<TEvent>(this IEventBus eventBus,Func<TEvent,Task> lambda)
            where TEvent : IEvent
        {
            var handler = new AsyncLambdaHandler<TEvent>(lambda);
            return eventBus.SubscribeAsync(handler);
        }
    }
}
