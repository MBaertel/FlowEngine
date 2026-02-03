using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.EventHandlers;
using TurnBasedEngine.Core.Events;

namespace TurnBasedEngine.Core.EventBus
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
            where TEvent : IGameEvent;

        /// <summary>
        /// Subscribe to the Event Bus with a Generic Handler.
        /// </summary>
        /// <returns>Unique Subscription Identifier</returns>
        Guid Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : IGameEvent;

        /// <summary>
        /// Subscribe to the Event Bus with a non-generic Async Handler.
        /// </summary>
        /// <returns>Unique Subscription Identifier</returns>
        Guid SubscribeAsync<TEvent>(IAsyncEventHandler handler)
            where TEvent : IGameEvent;

        /// <summary>
        /// Subscribe to the Event Bus with a Generic Async Handler.
        /// </summary>
        /// <returns>Unique Subscription Identifier</returns>
        Guid SubscribeAsync<TEvent>(IAsyncEventHandler<TEvent> handler)
            where TEvent : IGameEvent;

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
        void Publish(IGameEvent evt);

        /// <summary>
        /// Publish an event Asynchronously.
        /// </summary>
        Task PublishAsync(IGameEvent evt,CancellationToken cancellationToken = default);
    }
}
