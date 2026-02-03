using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Events
{
    public interface IAfterEvent<TEvent>
        where TEvent : IGameEvent
    {
        Type MainEventType { get; }
    }
}
