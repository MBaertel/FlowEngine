using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowOrchestrator : IFlowOrchestrator
    {
        private readonly IFlowDefinitionRegistry _definitionRegistry;

        private readonly List<IFlowRunner> _activeRunners = new();
        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _activeRunners.ToList().AsReadOnly();

        public FlowOrchestrator(IFlowDefinitionRegistry definitionRegistry)
        {
            _definitionRegistry = definitionRegistry;
        }

        private async Task<FlowValue> ExecuteFlowInternalAsync(
            IFlowDefinition flowDefinition,
            FlowValue input)
        {
            var context = new FlowContext(this,_definitionRegistry, input);
            var runner = new FlowRunner<FlowValue>(context, flowDefinition.Flow);

            try
            {
                _activeRunners.Add(runner);
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
            FlowValue result = await ExecuteFlowInternalAsync(flow, input);
            return result as TypedValue<TResult>;
        }

        //Output Only
        public async Task<TypedValue<TResult?>> ExecuteFlowAsync<TResult>(
            IFlowDefinition<EmptyValue, TResult> flow)
        {
            FlowValue result = await ExecuteFlowInternalAsync(flow, FlowValue.Empty);
            return result as TypedValue<TResult>;
        }

        //Input Only
        public async Task ExecuteFlowAsync<TInput>(IFlowDefinition<TInput, EmptyValue> flow, TypedValue<TInput> input)
        {
            await ExecuteFlowInternalAsync(flow, input);
        }

        //No Input or Output
        public async Task ExecuteFlowAsync(IFlowDefinition<EmptyValue, EmptyValue> flow)
        {
            await ExecuteFlowInternalAsync(flow, FlowValue.Empty);
        }

        public async Task<FlowValue> ExecuteFlowAsync(IFlowDefinition flow, FlowValue input)
        {
            return await ExecuteFlowInternalAsync(flow, input);
        }

        public async Task StepAllAsync()
        {
            foreach (var runner in _activeRunners)
            {
                if(!runner.IsWaiting)
                    await runner.StepAsync();
            }
        }
    }
}
