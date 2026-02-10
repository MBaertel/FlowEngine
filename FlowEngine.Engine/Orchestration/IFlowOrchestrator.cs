using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public interface IFlowOrchestrator
    {
        Task<FlowValue> ExecuteFlowAsync(IFlowDefinition flow,FlowValue input);
        Task<TypedValue<TResult>?> ExecuteFlowAsync<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TypedValue<TInput> input);

        Task<int> StepAllAsync();
        IReadOnlyCollection<IFlowRunner> ActiveRunners { get; }
    }
}
