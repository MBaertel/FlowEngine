using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Values
{
    public enum FlowStatus
    {
        Inactive,
        Running,
        Waiting,
        Completed,
        Canceled,
        Faulted,
    }
}
