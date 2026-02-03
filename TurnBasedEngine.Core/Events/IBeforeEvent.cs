using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Events
{
    public interface IBeforeEvent<TEvent>
        where TEvent : IGameEvent
    {
        Type MainEventType { get; }
    }
}
