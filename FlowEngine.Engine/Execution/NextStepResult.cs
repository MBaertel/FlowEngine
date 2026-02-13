using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution
{
    public sealed record NextStepResult (
        Guid StepNodeId,
        object Input);
}
