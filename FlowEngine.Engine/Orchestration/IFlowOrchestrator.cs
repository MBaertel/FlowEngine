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
        Task StepAllAsync(int maxRunnersPerTick = 1000);
        IReadOnlyCollection<IFlowRunner> ActiveRunners { get; }
        Task<object> ExecuteFlowAsync(IFlowDefinition flow, object input);
        Task<TResult> ExecuteFlowAsync<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TInput input);

        IFlowRunner AddFlow(IFlowDefinition flow,object input);
        IFlowRunner<TResult> AddFlow<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TInput input);

        public IFlowRunner<T> GetRunner<T>(Guid instanceId);
        public IFlowRunner GetRunner(Guid instanceId);

        public void EnqueueRunner(IFlowRunner runner);
    }
}
