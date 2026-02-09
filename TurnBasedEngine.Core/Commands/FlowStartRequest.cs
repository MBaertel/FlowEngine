using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Commands
{
    public class FlowStartRequest
    {
        public IFlowDefinition Definition { get; }
        public NodeValue Input { get; }

        public FlowStartRequest(IFlowDefinition definition, NodeValue input)
        {
            Definition = definition;
            Input = input;
        }
    }
}
