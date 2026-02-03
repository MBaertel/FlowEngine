using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Events
{
    public abstract class EventBase : IGameEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public virtual string Name => this.GetType().Name;
    }
}
