using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Instances
{
    public interface IFlowInstance
    {
        Guid Id { get; }
        IFlowDefinition Definition { get; }
        FlowContext Context { get; }
        IFlowStep CurrentStep { get; }
        StepValue Payload { get; }
        bool IsCompleted { get; }

        Task StepAsync();
    }
}
