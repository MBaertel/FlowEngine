using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using FlowEngine.Engine.Execution.Instances;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Execution
{
    public interface IFlowRunner
    {
        Guid Id { get; }
        public bool IsWaiting { get; }
        public IFlowInstance Flow { get; }
        public IFlowContext Context { get; }

        Task<object?> WaitForCompletion();
        ValueTask StepAsync();
    }

    public interface IFlowRunner<TIn,TResult> : IFlowRunner
    {
        new Task<TResult> WaitForCompletion();
    }
}
