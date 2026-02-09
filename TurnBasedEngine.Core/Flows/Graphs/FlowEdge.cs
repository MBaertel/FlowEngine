using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Graphs
{
    public sealed class FlowEdge
    {
        public Guid Id { get; }
        public Guid FromNodeId { get; }
        public Guid ToNodeId { get; }
        private Func<IFlowContext,bool>? _condition { get; }

        public FlowEdge(
            Guid id,
            Guid fromNodeId, 
            Guid toNodeId, 
            Func<IFlowContext, bool>? condition)
        {
            Id = id;
            FromNodeId = fromNodeId;
            ToNodeId = toNodeId;
            _condition = condition;
        }

        public bool CanTransition(IFlowContext ctx)
            => _condition?.Invoke(ctx) ?? true;
    }
}
