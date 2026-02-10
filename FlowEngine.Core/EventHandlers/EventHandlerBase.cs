using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public abstract class EventHandlerBase<TEvent>
        : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public bool CanHandle(IEvent evt) =>
            typeof(TEvent).IsAssignableFrom(evt.GetType());

        public abstract void Handle(TEvent evt);

        public void Handle(IEvent evt)
        {
            if(CanHandle(evt) && evt is TEvent tevt)
                Handle(tevt);
        }
    }
}
