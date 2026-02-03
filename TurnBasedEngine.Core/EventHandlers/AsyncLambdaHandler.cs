using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Events;

namespace TurnBasedEngine.Core.EventHandlers
{
    public class AsyncLambdaHandler<TEvent>
        : AsyncEventHandlerBase<TEvent>
        where TEvent : IGameEvent
    {
        private readonly Func<TEvent, CancellationToken, Task> _lambda;
        public AsyncLambdaHandler(Func<TEvent,Task> lambda)
        {
            _lambda = (evt, ct) => lambda(evt);
        }

        public override async Task HandleAsync(TEvent evt, CancellationToken cancellationToken = default)
        {
            await _lambda(evt,cancellationToken);
        }
    }
}
