using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using FlowEngine.Engine.Execution.Instances;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Steps;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowOrchestrator : IFlowOrchestrator
    {
        private readonly IFlowDefinitionRegistry _definitionRegistry;
        private readonly IStepFactory _stepFactory;

        private readonly List<IFlowRunner> _activeRunners = new();
        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _activeRunners.ToList().AsReadOnly();

        public FlowOrchestrator(IFlowDefinitionRegistry definitionRegistry,IStepFactory stepFactory)
        {
            _definitionRegistry = definitionRegistry;
            _stepFactory = stepFactory;
        }

        public async Task<FlowValue> ExecuteFlowAsync(
            IFlowDefinition flowDefinition,
            FlowValue input)
        {
            var context = new FlowContext(this,_definitionRegistry, input);
            var instance = new FlowInstance(_stepFactory, flowDefinition.Flow);
            var runner = new FlowRunner<FlowValue>(instance,context);

            _activeRunners.Add(runner);
            
            try
            {
                return await runner.WaitForCompletion();
            }
            finally
            {
                _activeRunners.Remove(runner);
            }
        }

        //Input and Output
        public async Task<TypedValue<TResult>?> ExecuteFlowAsync<TInput, TResult>(
            IFlowDefinition<TInput, TResult> flow, 
            TypedValue<TInput> input)
        {
            FlowValue result = await ExecuteFlowAsync(flow, input);
            return result as TypedValue<TResult>;
        }

        public async Task<int> StepAllAsync()
        {
            int ran = 0;
            foreach (var runner in _activeRunners.ToArray())
            {
                if(!runner.IsWaiting)
                    ran++;
                    await runner.StepAsync();
            }
            return ran;
        }
    }
}
