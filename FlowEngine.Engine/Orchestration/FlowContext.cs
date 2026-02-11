using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Text;
using FlowEngine.Engine.Execution.Instances;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Serialization;
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowContext : IFlowContext
    {
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly IFlowDefinitionRegistry _flowRegistry;
        private readonly ISubflowCallRegisty _subflowCallRegisty;
        private readonly IFlowInstance _instance;
        private readonly Dictionary<string, object?> _variables = new();
        private int _callId = 0;

        public FlowContext(IFlowOrchestrator orchestrator, IFlowInstance instance, IFlowDefinitionRegistry registry,ISubflowCallRegisty callRegistry, object payload)
        {
            _flowOrchestrator = orchestrator;
            _flowRegistry = registry;
            _instance = instance;
            _subflowCallRegisty = callRegistry;
            Payload = payload ?? VoidValue.Value;
        }

        private object _payload;
        public object Payload
        {
            get => _payload;
            set 
            {
                _payload = value; 
                _callId = 0;
            }
        }

        public IFlowDefinition? ResolveFlowDefinitionById(Guid id) =>
            _flowRegistry.GetById(id);

        public IFlowDefinition? ResolveFlowDefinitionByName(string name) =>
            _flowRegistry.GetByName(name);

        public IFlowDefinition<TIn, TOut>? ResolveFlowDefinitionById<TIn, TOut>(Guid id) =>
            _flowRegistry.GetById<TIn, TOut>(id);

        public IFlowDefinition<TIn, TOut>? ResolveFlowDefinitionByName<TIn, TOut>(string name) =>
            _flowRegistry.GetByName<TIn, TOut>(name);

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

        public FlowWait ExecuteSubFlow(IFlowDefinition definition,object input)
        {
            var callKey = new SubflowCallKey(
                _instance.InstanceId,
                _flowOrchestrator.GetRunner(_instance.InstanceId).GetCurrentStepId(),
                _callId);

            if (!_subflowCallRegisty.TryGet(callKey, out var wait))
            {
                var runner = _flowOrchestrator.AddFlow(definition, input);
                wait = new FlowWait(runner.Flow.InstanceId);
                wait.BindRunner(runner);
            }
            else
            {
                var runner = _flowOrchestrator.GetRunner(wait.FlowInstanceId);
                wait.BindRunner(runner);
            }

            _callId++;
            return wait;
        }

        public FlowWait<TResult> ExecuteSubFlow<TInput,TResult>(IFlowDefinition<TInput,TResult> definition,TInput input)
        {
            var callKey = new SubflowCallKey(
                _instance.InstanceId,
                _flowOrchestrator.GetRunner(_instance.InstanceId).GetCurrentStepId(),
                _callId);


            if(!_subflowCallRegisty.TryGet(callKey,out var wait))
            {
                var runner = _flowOrchestrator.AddFlow(definition, input);
                wait = new FlowWait<TResult>(runner.Flow.InstanceId);
                wait.BindRunner(runner);
            }
            else
            {
                var runner = _flowOrchestrator.GetRunner(wait.FlowInstanceId);
                wait.BindRunner(runner);
            }

            _callId++;
            return (FlowWait<TResult>)wait;
        }
    }
}
