using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TurnBasedEngine.Core.Commands;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Execution;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Orchestration
{
    public class FlowContext : IFlowContext
    {
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly IFlowDefinitionRegistry _flowRegistry;
        private readonly Dictionary<string, object?> _variables = new();

        public FlowContext(IFlowOrchestrator orchestrator,IFlowDefinitionRegistry registry,NodeValue payload)
        {
            _flowOrchestrator = orchestrator;
            _flowRegistry = registry;
            Payload = payload ?? NodeValue.Empty;
        }

        public NodeValue Payload { get; set; }

        public IFlowDefinition ResolveFlowDefinitionById(Guid id) =>
            _flowRegistry.GetById(id);

        public IFlowDefinition ResolveFlowDefinitionByName(string name) =>
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

        public async Task<NodeValue> ExecuteSubFlow(IFlowDefinition definition,NodeValue data) =>
            await _flowOrchestrator.ExecuteFlowAsync(definition, data);

        public async Task<TypedValue<TResult>> ExecuteSubFlow<TResult>(IFlowDefinition definition,NodeValue data)
        {
            var result = await _flowOrchestrator.ExecuteFlowAsync(definition,data);
            if(result is TypedValue<TResult> typed)
                return typed;
            throw new InvalidCastException($"Flow returned value of type {result.GetType()}, expected {typeof(TypedValue<TResult>)}");
        }
    }
}
