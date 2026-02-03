using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Events
{
    public class BeforeEventBase<TEvent> : EventBase, IBeforeEvent<TEvent>
        where TEvent : IGameEvent
    {
        public virtual Type MainEventType => typeof(TEvent);
    }
}
