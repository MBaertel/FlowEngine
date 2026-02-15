using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution
{
    public interface IFlowInstance
    {
        Guid InstanceId { get; }
        IFlowDefinition Definition { get; }
        
        Guid StartStepId { get; }
        Guid CurrentStepId { get; set; }

        FlowStatus Status { get; set; }

        object Payload { get; set; }
        IDictionary<string,object?> Variables { get; }
        
        IList<HistoryFrame> History { get; }

        IDictionary<SubflowCallKey,FlowAwaiterBase> ActiveAwaiters { get; }

        IFlowStep GetStep(Guid stepId);
        bool TryResolveNext(Guid currentStepId, out NextStepResult next);

        void RegisterSubflowCall(SubflowCallKey key, Guid childInstanceId);
        bool TryGetSubflowCall(SubflowCallKey key, out Guid childInstanceId);
    }
}
