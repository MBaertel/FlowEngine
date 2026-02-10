using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public class LambdaEventHandler<TEvent>
        : EventHandlerBase<TEvent>
        where TEvent : IEvent
    {
        private readonly Action<TEvent> _lambda;
        public LambdaEventHandler(Action<TEvent> lambda)
        {
            _lambda = lambda;
        }

        public override void Handle(TEvent evt) => _lambda(evt);
    }
}
