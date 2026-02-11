using FlowEngine.Engine.Flows.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Values
{
    public sealed class FlowWait
    {
        public IFlowRunner SubflowRunner { get; }

        public FlowWait(IFlowRunner subflowRunner)
        {
            SubflowRunner = subflowRunner;
        }

        public Task<object?> WaitForCompletion() =>
            SubflowRunner.WaitForCompletion();
    }
}
