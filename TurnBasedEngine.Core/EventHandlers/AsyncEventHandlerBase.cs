using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Events;

namespace TurnBasedEngine.Core.EventHandlers
{
    public abstract class AsyncEventHandlerBase<TEvent>
    : IAsyncEventHandler<TEvent>
    where TEvent : IGameEvent
    {
        public bool CanHandle(IGameEvent evt) =>
            typeof(TEvent).IsAssignableFrom(evt.GetType());

        public abstract Task HandleAsync(TEvent evt, CancellationToken cancellationToken = default);

        public async Task HandleAsync(IGameEvent evt, CancellationToken cancellationToken = default)
        {
            if(CanHandle(evt) && evt is TEvent tevt)
                await HandleAsync(tevt,cancellationToken);
        }

        //Sync interface Fallback
        public void Handle(IGameEvent evt) => throw new NotSupportedException();
        public void Handle(TEvent evt) => throw new NotSupportedException();
    }
}
