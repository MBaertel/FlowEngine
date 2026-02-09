using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Execution;
using TurnBasedEngine.Core.Flows.Steps;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Orchestration
{
    public interface IFlowOrchestrator
    {
        Task<NodeValue> ExecuteFlowAsync(IFlowDefinition flow,NodeValue input);
        Task<TypedValue<TResult>?> ExecuteFlowAsync<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TypedValue<TInput> input);
        Task<TypedValue<TResult?>> ExecuteFlowAsync<TResult>(IFlowDefinition<EmptyValue, TResult> flow);
        Task ExecuteFlowAsync<TInput>(IFlowDefinition<TInput,EmptyValue> flow,TypedValue<TInput> input);
        Task ExecuteFlowAsync(IFlowDefinition<EmptyValue,EmptyValue> flow);

        Task StepAllAsync();

        IReadOnlyCollection<IFlowRunner> ActiveRunners { get; }
    }
}
