using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs
{
    public sealed class FlowNode
    {
        public Guid Id { get; }

        public Type StepType { get; }

        public FlowNode(Guid id, Type step)
        {
            Id = id;
            StepType = step;
        }

        public FlowNode From<TStep>(Guid id,TStep step)
            where TStep : IFlowStep
        {
            return new FlowNode(id, typeof(TStep));
        }
    }
}
