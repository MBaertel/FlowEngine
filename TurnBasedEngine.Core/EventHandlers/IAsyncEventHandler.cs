using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public interface IAsyncEventHandler
    : IEventHandler
    {
        Task HandleAsync(IEvent evt,CancellationToken cancellationToken = default);
    }

    public interface IAsyncEventHandler<TEvent>
        : IEventHandler<TEvent>, IAsyncEventHandler
        where TEvent : IEvent
    {
        Task HandleAsync(TEvent evt, CancellationToken cancellationToken = default);
    }
}
