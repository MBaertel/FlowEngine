using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Graphs
{
    public sealed class FlowNode
    {
        public Guid Id { get; }

        public IFlowStep Step { get; }
        public Func<IFlowContext,object?,object?>? InputResolver { get; }

        public FlowNode(Guid id, IFlowStep step, Func<IFlowContext,object?, object?>? inputResolver = null)
        {
            Id = id;
            Step = step;
            InputResolver = inputResolver;
        }
    }
}
