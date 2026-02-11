using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Serialization
{
    public readonly record struct SubflowCallKey(
        Guid FlowInstanceId,
        Guid StepId,
        int CallIndex);
}
