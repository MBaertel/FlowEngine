using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Events;

namespace TurnBasedEngine.Core.EventHandlers
{
    public interface IEventHandler
    {
        bool CanHandle(IGameEvent evt);
        void Handle(IGameEvent evt);
    }

    public interface IEventHandler<TEvent>
        : IEventHandler
        where TEvent : IGameEvent
    {
        void Handle(TEvent evt);
    }
}
