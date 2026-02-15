using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Events
{
    public abstract class EventBase : IFlowEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public virtual string Name => this.GetType().Name;
    }

    public abstract class BeforeEventBase<TEvent>
        : EventBase, IBeforeEvent
        where TEvent : IFlowEvent
    {
        public Type MainEventType => typeof(TEvent);
    }

    public abstract class AfterEventBase<TEvent>
    : EventBase, IAfterEvent
    where TEvent : IFlowEvent
    {
        public Type MainEventType => typeof(TEvent);
    }
}
