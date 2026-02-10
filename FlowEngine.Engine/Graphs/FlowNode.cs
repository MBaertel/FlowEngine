using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs
{
    public class FlowNode
    {
        public Guid Id { get; }

        public Type StepType { get; }

        public IFlowStep Step { get; set; }

        public FlowNode(Guid id, Type step)
        {
            Id = id;
            StepType = step;
        }
    }

    public sealed class FlowNode<TStep> : FlowNode
        where TStep : IFlowStep
    {
        public FlowNode(Guid id)
        :base(id,typeof(TStep)) { }
    }
}
