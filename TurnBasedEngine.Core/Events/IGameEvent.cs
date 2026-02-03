using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Events
{
    public interface IGameEvent
    { 
        Guid Id { get; }
        string Name { get; }
    }
}
