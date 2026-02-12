using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs
{
    public sealed class FlowEdge
    {
        public Guid Id { get; }
        public Guid FromNodeId { get; }
        public Guid ToNodeId { get; }
        private Func<IFlowContext,object,bool>? _condition { get; }

        public FlowEdge(
            Guid id,
            Guid fromNodeId, 
            Guid toNodeId, 
            Func<IFlowContext,object, bool>? condition = null)
        {
            Id = id;
            FromNodeId = fromNodeId;
            ToNodeId = toNodeId;
            _condition = condition;
        }

        public bool CanTransition(IFlowContext ctx,object payload)
            => _condition?.Invoke(ctx,payload) ?? true;
    }
}
