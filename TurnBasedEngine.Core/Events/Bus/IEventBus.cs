using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.EventHandlers;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.Events.Bus
{
    /// <summary>
    /// Event bus to handle Cross Cutting Communication Concerns.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Subscribe to the Event Bus with a non-generic Handler.
        /// </summary>
        /// <returns>Unique Subscription Identifier</returns>
        Guid Subscribe<TEvent>(IEventHandler handler)
            where TEvent : IEvent;

        /// <summary>
        /// Subscribe to the Event Bus with a Generic Handler.
        /// </summary>
        /// <returns>Unique Subscription Identifier</returns>
        Guid Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IEvent;

        /// <summary>
        /// Subscribe to the Event Bus with a non-generic Async Handler.
        /// </summary>
        /// <returns>Unique Subscription Identifier</returns>
        Guid SubscribeAsync<TEvent>(IAsyncEventHandler handler)
            where TEvent : IEvent;

        /// <summary>
        /// Subscribe to the Event Bus with a Generic Async Handler.
        /// </summary>
        /// <returns>Unique Subscription Identifier</returns>
        Guid SubscribeAsync<TEvent>(IAsyncEventHandler<TEvent> handler)
            where TEvent : IEvent;

        /// <summary>
        /// Unsubscribe from the bus via Handler Reference.
        /// </summary>
        void Unsubscribe(IEventHandler handler);

        /// <summary>
        /// Unsubscribe from the bus via Subscription Id.
        /// </summary>
        void Unsubscribe(Guid subId);

        /// <summary>
        /// Publish an event Synchronously.
        /// </summary>
        void Publish(IEvent evt);

        /// <summary>
        /// Publish an event Asynchronously.
        /// </summary>
        Task PublishAsync(IEvent evt,CancellationToken cancellationToken = default);
    }
}
