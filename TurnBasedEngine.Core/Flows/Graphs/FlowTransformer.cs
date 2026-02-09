using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Graphs
{
    public sealed class FlowTransformer
    {
        public Guid NodeId { get; set; }

        private Func<IFlowContext, NodeValue> _transformFunction;

        public FlowTransformer(
            Guid edgeId,
            Func<IFlowContext, NodeValue> transformFunction)
        {
            _transformFunction = transformFunction;
        }

        public NodeValue Transform(IFlowContext context) =>
            _transformFunction(context);
    }
}
