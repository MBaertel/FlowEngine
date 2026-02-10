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

        public FlowContext(IFlowOrchestrator orchestrator,IFlowDefinitionRegistry registry,FlowValue payload)
        {
            _flowOrchestrator = orchestrator;
            _flowRegistry = registry;
            Payload = payload ?? FlowValue.Empty;
        }

        public FlowValue Payload { get; set; }

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

        public async Task<FlowValue> ExecuteSubFlow(IFlowDefinition definition,FlowValue data) =>
            await _flowOrchestrator.ExecuteFlowAsync(definition, data);

        public async Task<TypedValue<TResult>> ExecuteSubFlow<TResult>(IFlowDefinition definition,FlowValue data)
        {
            var result = await _flowOrchestrator.ExecuteFlowAsync(definition,data);
            if(result is TypedValue<TResult> typed)
                return typed;
            throw new InvalidCastException($"Flow returned value of type {result.GetType()}, expected {typeof(TypedValue<TResult>)}");
        }
    }
}
