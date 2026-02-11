using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution.Instances
{
    public interface IFlowInstance
    {
        public Guid InstanceId { get; }
        public Guid StartStepId { get; }
        public IFlowStep GetStep(Guid stepId);
        public NextStepResult ResolveNext(Guid currentStepId, IFlowContext ctx);
    }
}
