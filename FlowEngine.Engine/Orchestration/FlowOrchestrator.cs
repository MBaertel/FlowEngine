using FlowEngine.Engine.Execution.Instances;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Steps;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FlowEngine.Engine.Values;
using FlowEngine.Engine.Serialization;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowOrchestrator : IFlowOrchestrator
    {
        private readonly ISubflowCallRegisty _callRegistry;
        private readonly IFlowDefinitionRegistry _definitionRegistry;
        private readonly IStepFactory _stepFactory;

        private readonly Dictionary<Guid, IFlowRunner> _activeRunners = new();
        private readonly List<IFlowRunner> _runnersToStep = new();
        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _activeRunners.Values.ToList().AsReadOnly();

        public FlowOrchestrator(IFlowDefinitionRegistry definitionRegistry,IStepFactory stepFactory,ISubflowCallRegisty callRegisty)
        {
            _definitionRegistry = definitionRegistry;
            _stepFactory = stepFactory;
            _callRegistry = callRegisty;
        }

        public IFlowRunner AddFlow(IFlowDefinition flow, object input)
        {
            var instance = new FlowInstance(_stepFactory, flow.Flow);
            var context = new FlowContext(this, instance, _definitionRegistry,_callRegistry, input);
            var runner = new FlowRunner<object>(instance, context);

            _activeRunners.Add(runner.RunnerId,runner);
            return runner;
        }

        public IFlowRunner<TResult> AddFlow<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TInput input)
        {
            var instance = new FlowInstance(_stepFactory, flow.Flow);
            var context = new FlowContext(this,instance, _definitionRegistry, _callRegistry, input);
            var runner = new FlowRunner<TResult>(instance, context);

            _activeRunners.Add(runner.RunnerId,runner);
            return runner;
        }

        public async Task<object> ExecuteFlowAsync(
            IFlowDefinition flowDefinition,
            object input)
        {
            var runner = AddFlow(flowDefinition, input);
            
            return await runner.WaitForCompletion();
        }

        //Input and Output
        public async Task<TResult> ExecuteFlowAsync<TInput, TResult>(
            IFlowDefinition<TInput, TResult> flow, 
            TInput input)
        {
            var runner = AddFlow(flow, input);

            return await runner.WaitForCompletion();
        }

        public Task StepAllAsync()
        {
            _runnersToStep.Clear();
            _runnersToStep.AddRange(_activeRunners.Values);

            foreach (var runner in _runnersToStep)
            {
                if(runner != null && !runner.IsWaiting && !runner.IsCompleted)
                {
                    //_ = StepRunnerSafelyAsync(runner);
                    _ = StepRunnerSafelyAsync(runner);
                }
            }
            var removeIds = _activeRunners.Values.Where(x => x.IsCompleted).Select(x => x.RunnerId);
            foreach (var id in removeIds)
            {
                _activeRunners.Remove(id);
            }
            return Task.CompletedTask;
        }

        private async ValueTask StepRunnerSafelyAsync(IFlowRunner runner)
        {
            try
            {
                await runner.StepAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        public IFlowRunner<T>? GetRunner<T>(Guid instanceId)
        {
            if(_activeRunners.TryGetValue(instanceId, out var runner) && runner is IFlowRunner<T> typedRunner)
                return typedRunner;
            return null;
        }

        public IFlowRunner? GetRunner(Guid instanceId)
        {
            if (_activeRunners.TryGetValue(instanceId, out var runner))
                return runner;
            return null;
        }
    }
}
