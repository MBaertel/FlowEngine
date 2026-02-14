using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public abstract class AsyncEventHandlerBase<TEvent>
    : IAsyncEventHandler<TEvent>
    where TEvent : IFlowEvent
    {
        public bool CanHandle(IFlowEvent evt) =>
            typeof(TEvent).IsAssignableFrom(evt.GetType());

        public abstract Task HandleAsync(TEvent evt, CancellationToken cancellationToken = default);

        public async Task HandleAsync(IFlowEvent evt, CancellationToken cancellationToken = default)
        {
            if(CanHandle(evt) && evt is TEvent tevt)
                await HandleAsync(tevt,cancellationToken);
        }

        //Sync interface Fallback
        public void Handle(IFlowEvent evt) => throw new NotSupportedException();
        public void Handle(TEvent evt) => throw new NotSupportedException();
    }
}
