using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Events
{
    public interface IFlowEvent
    { 
        Guid Id { get; }
        string Name { get; }
    }

    public interface IBeforeEvent : IFlowEvent
    {
        Type MainEventType { get; }
    }

    public interface IAfterEvent : IFlowEvent
    {
        Type MainEventType { get; }
    }
}
