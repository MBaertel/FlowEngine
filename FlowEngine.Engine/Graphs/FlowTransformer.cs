using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs
{
    public sealed class FlowTransformer
    {
        public Guid EdgeId { get; }

        private readonly Func<IFlowContext, object,object> _transformFunction;

        public FlowTransformer(
            Guid edgeId,
            Func<IFlowContext, object,object> transformFunction)
        {
            EdgeId = edgeId;
            _transformFunction = transformFunction;
        }

        public object Transform(IFlowContext context,object payload) =>
            _transformFunction(context,payload);
    }
}
