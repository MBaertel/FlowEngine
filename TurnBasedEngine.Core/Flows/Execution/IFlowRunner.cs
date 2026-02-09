using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Graphs;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Execution
{
    public interface IFlowRunner
    {
        public bool IsWaiting { get; }
        public FlowGraph Flow { get; }
        public IFlowContext Context { get; }

        Task<object?> WaitForCompletion();
        Task StepAsync();
    }

    public interface IFlowRunner<TResult> : IFlowRunner
        where TResult : NodeValue
    {
        new Task<TResult> WaitForCompletion();
    }
}
