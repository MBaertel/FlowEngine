using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Events
{
    public abstract class EventBase : IEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public virtual string Name => this.GetType().Name;
    }
}
