using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public abstract class EventHandlerBase<TEvent>
        : IEventHandler<TEvent>
        where TEvent : IFlowEvent
    {
        public bool CanHandle(IFlowEvent evt) =>
            typeof(TEvent).IsAssignableFrom(evt.GetType());

        public abstract void Handle(TEvent evt);

        public void Handle(IFlowEvent evt)
        {
            if(CanHandle(evt) && evt is TEvent tevt)
                Handle(tevt);
        }
    }
}
