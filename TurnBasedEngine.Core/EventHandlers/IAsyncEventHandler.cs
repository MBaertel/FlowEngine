using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Events;

namespace TurnBasedEngine.Core.EventHandlers
{
    public interface IAsyncEventHandler
    : IEventHandler
    {
        Task HandleAsync(IGameEvent evt,CancellationToken cancellationToken = default);
    }

    public interface IAsyncEventHandler<TEvent>
        : IEventHandler<TEvent>, IAsyncEventHandler
        where TEvent : IGameEvent
    {
        Task HandleAsync(TEvent evt, CancellationToken cancellationToken = default);
    }
}
