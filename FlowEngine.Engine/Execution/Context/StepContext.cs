using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution.Context
{
    public class StepContext : IStepContext
    {
        private readonly IFlowInstance _instance;
        private readonly IFlowOrchestrator _orchestrator;
        private readonly IFlowDefinitionRegistry _flowRegisty;

        private readonly Guid _stepId;
        private int _callId = 0;

        public StepContext(IFlowInstance instance,IFlowOrchestrator orchestrator,IFlowDefinitionRegistry registry,Guid stepId) 
        {
            _orchestrator = orchestrator;
            _flowRegisty = registry;
            _instance = instance;
            _stepId = stepId;
        }

        public IDictionary<string, object?> UndoRequiredValues { get; } = new Dictionary<string, object?>();

        public async Task<object> ExecuteSubflow(IFlowDefinition flowDefinition, object input)
        {
            throw new NotImplementedException();
        }

        public Task<TOut> ExecuteSubFlow<TIn, TOut>(IFlowDefinition<TIn, TOut> flowDefinition, TIn input)
        {
            FlowAwaiter<TOut> finalAwaiter;
            var subflowCallKey = new SubflowCallKey(flowDefinition.Id, _callId);
            if (_instance.ActiveAwaiters.TryGetValue(subflowCallKey, out var awaiter)
                && awaiter != null)
            {
                var runner = _orchestrator.GetRunner(awaiter.SubflowInstanceId);
                finalAwaiter = (FlowAwaiter<TOut>)awaiter;
                finalAwaiter.ConnectRunner(runner);
            }
            else
            {
                var runner = _orchestrator.AddFlow(flowDefinition,input);
                finalAwaiter = new FlowAwaiter<TOut>(runner);
                _instance.ActiveAwaiters[subflowCallKey] = finalAwaiter;
            }

            return finalAwaiter.AsTask();
        }

        public IFlowDefinition GetFlowById(Guid id) =>
            _flowRegisty.GetById(id);

        public IFlowDefinition<TIn, TOut> GetFlowById<TIn, TOut>(Guid id) =>
            _flowRegisty.GetById<TIn, TOut>(id);

        public IFlowDefinition GetFlowByName(string name) =>
            _flowRegisty.GetByName(name);

        public IFlowDefinition<TIn, TOut> GetFlowByName<TIn, TOut>(string name) =>
            _flowRegisty.GetByName<TIn, TOut>(name);
    }
}
