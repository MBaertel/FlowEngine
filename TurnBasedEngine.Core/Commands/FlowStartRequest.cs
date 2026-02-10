using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Core.Commands
{
    public class FlowStartRequest
    {
        public IFlowDefinition Definition { get; }
        public FlowValue Input { get; }

        public FlowStartRequest(IFlowDefinition definition, FlowValue input)
        {
            Definition = definition;
            Input = input;
        }
    }
}
