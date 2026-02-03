using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Events;

namespace TurnBasedEngine.Core.EventHandlers
{
    public abstract class EventHandlerBase<TEvent>
        : IEventHandler<TEvent>
        where TEvent : IGameEvent
    {
        public bool CanHandle(IGameEvent evt) =>
            typeof(TEvent).IsAssignableFrom(evt.GetType());

        public abstract void Handle(TEvent evt);

        public void Handle(IGameEvent evt)
        {
            if(CanHandle(evt) && evt is TEvent tevt)
                Handle(tevt);
        }
    }
}
