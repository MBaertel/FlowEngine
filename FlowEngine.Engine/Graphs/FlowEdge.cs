using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs
{
    public sealed class FlowEdge
    {
        public Guid Id { get; }
        public Guid FromNodeId { get; }
        public Guid ToNodeId { get; }
        private Func<IFlowContext,FlowValue,bool>? _condition { get; }

        public FlowEdge(
            Guid id,
            Guid fromNodeId, 
            Guid toNodeId, 
            Func<IFlowContext,FlowValue, bool>? condition = null)
        {
            Id = id;
            FromNodeId = fromNodeId;
            ToNodeId = toNodeId;
            _condition = condition;
        }

        public bool CanTransition(IFlowContext ctx,FlowValue payload)
            => _condition?.Invoke(ctx,payload) ?? true;
    }
}
