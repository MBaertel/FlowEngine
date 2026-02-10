using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public interface IEventHandler
    {
        bool CanHandle(IEvent evt);
        void Handle(IEvent evt);
    }

    public interface IEventHandler<TEvent>
        : IEventHandler
        where TEvent : IEvent
    {
        void Handle(TEvent evt);
    }
}
