using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Instances;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Factories
{
    public interface IFlowFactory
    {
        public IFlowInstance CreateInstance(IFlowDefinition definition, StepValue input);
    }
}
