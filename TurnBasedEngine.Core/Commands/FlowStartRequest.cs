using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Instances;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Commands
{
    public class FlowStartRequest
    {
        public IFlowDefinition Definition { get; }
        public StepValue Input { get; }

        public FlowStartRequest(IFlowDefinition definition, StepValue input)
        {
            Definition = definition;
            Input = input;
        }
    }
}
