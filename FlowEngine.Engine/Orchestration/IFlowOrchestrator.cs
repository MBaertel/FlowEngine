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
        Task<TypedValue<TResult?>> ExecuteFlowAsync<TResult>(IFlowDefinition<EmptyValue, TResult> flow);
        Task ExecuteFlowAsync<TInput>(IFlowDefinition<TInput,EmptyValue> flow,TypedValue<TInput> input);
        Task ExecuteFlowAsync(IFlowDefinition<EmptyValue,EmptyValue> flow);

        Task StepAllAsync();

        IReadOnlyCollection<IFlowRunner> ActiveRunners { get; }
    }
}
