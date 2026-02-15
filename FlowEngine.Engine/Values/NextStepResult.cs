using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Values
{
    public sealed record NextStepResult (
        Guid StepId,
        object Input);
}
