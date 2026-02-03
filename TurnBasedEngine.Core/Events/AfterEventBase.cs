using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Events
{
    public class AfterEventBase<TEvent> : EventBase, IAfterEvent<TEvent>
        where TEvent : IGameEvent
    {
        public virtual Type MainEventType => typeof(TEvent);
    }
}
