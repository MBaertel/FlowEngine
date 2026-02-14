using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Steps;
using FlowEngine.Engine.Execution;
using FlowEngine.Engine.Execution.Context;
using System.Collections.Concurrent;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowOrchestrator : IFlowOrchestrator
    {
        private readonly IFlowDefinitionRegistry _definitionRegistry;
        private readonly IStepFactory _stepFactory;

        private readonly Dictionary<Guid, IFlowRunner> _runnersById = new();
        private readonly List<IFlowRunner> _activeRunners = new();

        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _runnersById.Values.ToList().AsReadOnly();

        public FlowOrchestrator(IFlowDefinitionRegistry definitionRegistry,IStepFactory stepFactory)
        {
            _definitionRegistry = definitionRegistry;
            _stepFactory = stepFactory;
        }

        public IFlowRunner AddFlow(IFlowDefinition flow, object input)
        {
            var instance = new FlowInstance(flow, _stepFactory,input);
            var context = new FlowContext(instance);
            var runner = new FlowRunner<object>(instance,context,this,_definitionRegistry);

            _runnersById.Add(runner.InstanceId,runner);
            _activeRunners.Add(runner);
            return runner;
        }

        public IFlowRunner<TResult> AddFlow<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TInput input)
        {
            var instance = new FlowInstance(flow, _stepFactory,input);
            var context = new FlowContext(instance);
            var runner = new FlowRunner<TResult>(instance, context, this, _definitionRegistry);

            _runnersById.Add(runner.InstanceId,runner);
            _activeRunners.Add(runner);
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

        public async Task StepAllAsync()
        {
            for (int i = 0; i < _activeRunners.Count; i++)
            {
                var runner = _activeRunners[i];
                if (runner == null || !runner.IsWaiting)
                    runner.StepAsync();

                if (runner.IsCompleted)
                {
                    _activeRunners.Remove(runner);
                    _runnersById.Remove(runner.InstanceId);
                }
            }
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
            if(_runnersById.TryGetValue(instanceId, out var runner) && runner is IFlowRunner<T> typedRunner)
                return typedRunner;
            return null;
        }

        public IFlowRunner? GetRunner(Guid instanceId)
        {
            if (_runnersById.TryGetValue(instanceId, out var runner))
                return runner;
            return null;
        }
    }
}
