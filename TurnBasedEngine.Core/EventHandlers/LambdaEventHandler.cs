using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Events;

namespace TurnBasedEngine.Core.EventHandlers
{
    public class LambdaEventHandler<TEvent>
        : EventHandlerBase<TEvent>
        where TEvent : IGameEvent
    {
        private readonly Action<TEvent> _lambda;
        public LambdaEventHandler(Action<TEvent> lambda)
        {
            _lambda = lambda;
        }

        public override void Handle(TEvent evt) => _lambda(evt);
    }
}
