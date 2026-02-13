using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using FlowEngine.Engine.Execution;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Execution
{
    public interface IFlowRunner
    {
        Guid InstanceId { get; }

        IFlowInstance Instance { get; }

        bool IsWaiting { get; }
        bool IsCompleted { get; }
        
        Task StepAsync();
        Task<object> WaitForCompletion();
    }

    public interface IFlowRunner<TResult> : IFlowRunner
    {
        new Task<TResult> WaitForCompletion();
    }
}
