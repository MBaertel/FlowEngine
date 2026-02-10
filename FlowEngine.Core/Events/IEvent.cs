using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Events
{
    public interface IEvent
    { 
        Guid Id { get; }
        string Name { get; }
    }
}
