using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using FlowEngine.Engine.Execution;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Flows.Execution
{
    public interface IFlowRunner
    {
        IFlowInstance Instance { get; }

        FlowStatus Status { get; }
     
        Task StepAsync(CancellationToken ct = default);

        Task UndoAsync(int steps = int.MaxValue, CancellationToken ct = default);

        Task<object> WaitForCompletion();
    }

    public interface IFlowRunner<TResult> : IFlowRunner
    {
        new Task<TResult> WaitForCompletion();
    }
}
