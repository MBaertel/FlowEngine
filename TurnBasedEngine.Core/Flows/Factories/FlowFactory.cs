using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Instances;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Factories
{
    public class FlowFactory : IFlowFactory
    {
        private readonly IStepFactory _stepFactory;

        public FlowFactory(IStepFactory stepFactory)
        {
            _stepFactory = stepFactory;
        }

        public IFlowInstance CreateInstance(IFlowDefinition definition,StepValue input)
        {
            
        }
    }
}
