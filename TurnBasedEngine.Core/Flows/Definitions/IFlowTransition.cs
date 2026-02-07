using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Definitions
{
    public interface IFlowTransition
    {
        Guid FromNode { get; }

        IFlowStep? ResolveNextStep(StepValue previousOutput);
    }
}
