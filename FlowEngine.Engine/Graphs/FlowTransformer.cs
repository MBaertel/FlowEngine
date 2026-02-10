using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs
{
    public sealed class FlowTransformer
    {
        public Guid EdgeId { get; }

        private readonly Func<IFlowContext, FlowValue,FlowValue> _transformFunction;

        public FlowTransformer(
            Guid edgeId,
            Func<IFlowContext, FlowValue,FlowValue> transformFunction)
        {
            EdgeId = edgeId;
            _transformFunction = transformFunction;
        }

        public FlowValue Transform(IFlowContext context,FlowValue payload) =>
            _transformFunction(context,payload);
    }
}
