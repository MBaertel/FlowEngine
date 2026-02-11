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
        Task StepAllAsync();
        IReadOnlyCollection<IFlowRunner> ActiveRunners { get; }
        Task<object> ExecuteFlowUntypedAsync(IFlowDefinition flow, object input);
        Task<TResult> ExecuteFlowAsync<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TInput input);
    }
}
