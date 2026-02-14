using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Core.Events;

namespace FlowEngine.Core.EventHandlers
{
    public interface IEventHandler
    {
        bool CanHandle(IFlowEvent evt);
        void Handle(IFlowEvent evt);
    }

    public interface IEventHandler<TEvent>
        : IEventHandler
        where TEvent : IFlowEvent
    {
        void Handle(TEvent evt);
    }
}
