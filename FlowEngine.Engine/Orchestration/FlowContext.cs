using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowContext : IFlowContext
    {
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly IFlowDefinitionRegistry _flowRegistry;
        private readonly Dictionary<string, object?> _variables = new();

        public FlowContext(IFlowOrchestrator orchestrator,IFlowDefinitionRegistry registry,object payload)
        {
            _flowOrchestrator = orchestrator;
            _flowRegistry = registry;
            Payload = payload ?? VoidValue.Value;
        }

        public object Payload { get; set; }

        public IFlowDefinition? ResolveFlowDefinitionById(Guid id) =>
            _flowRegistry.GetById(id);

        public IFlowDefinition? ResolveFlowDefinitionByName(string name) =>
            _flowRegistry.GetByName(name);

        public T GetVar<T>(string name)
        {
            if(_variables.TryGetValue(name, out var value) && value is T tValue)
                return tValue;

            throw new InvalidCastException($"Variable {name} is not of type {typeof(T)}.");
        }

        public void SetVar<T>(string name, T value)
        {
            _variables[name] = value;
        }

        public async Task<object> ExecuteSubFlow(IFlowDefinition definition,object input) =>
            await _flowOrchestrator.ExecuteFlowUntypedAsync(definition,input);

        public async Task<TResult> ExecuteSubFlow<TInput,TResult>(IFlowDefinition<TInput,TResult> definition,TInput input)
        {
            var result = await _flowOrchestrator.ExecuteFlowAsync(definition,input);
            if(result is TResult typed)
                return typed;
            throw new InvalidCastException($"Flow returned value of type {result.GetType()}, expected {typeof(TResult)}");
        }
    }
}
