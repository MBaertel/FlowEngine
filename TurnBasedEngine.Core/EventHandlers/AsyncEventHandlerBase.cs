using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public abstract class AsyncEventHandlerBase<TEvent>
    : IAsyncEventHandler<TEvent>
    where TEvent : IEvent
    {
        public bool CanHandle(IEvent evt) =>
            typeof(TEvent).IsAssignableFrom(evt.GetType());

        public abstract Task HandleAsync(TEvent evt, CancellationToken cancellationToken = default);

        public async Task HandleAsync(IEvent evt, CancellationToken cancellationToken = default)
        {
            if(CanHandle(evt) && evt is TEvent tevt)
                await HandleAsync(tevt,cancellationToken);
        }

        //Sync interface Fallback
        public void Handle(IEvent evt) => throw new NotSupportedException();
        public void Handle(TEvent evt) => throw new NotSupportedException();
    }
}
