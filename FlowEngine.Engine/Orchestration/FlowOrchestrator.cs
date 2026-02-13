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

        private readonly Dictionary<Guid, IFlowRunner> _activeRunners = new();
        private readonly ConcurrentQueue<IFlowRunner> _readyRunners = new();
        private List<Task> _runningSteps = new();

        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _activeRunners.Values.ToList().AsReadOnly();

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

            _activeRunners.Add(runner.InstanceId,runner);
            _readyRunners.Enqueue(runner);
            return runner;
        }

        public IFlowRunner<TResult> AddFlow<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TInput input)
        {
            var instance = new FlowInstance(flow, _stepFactory,input);
            var context = new FlowContext(instance);
            var runner = new FlowRunner<TResult>(instance, context, this, _definitionRegistry);

            _activeRunners.Add(runner.InstanceId,runner);
            _readyRunners.Enqueue(runner);
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

        public async Task StepAllAsync(int maxRunnersPerTick = 1000)
        {
            int stepped = 0;
            while (_readyRunners.TryDequeue(out var runner) && stepped < maxRunnersPerTick)
            {
                if (runner == null) continue;
                await StepRunnerSafelyAsync(runner);

                if (runner.IsCompleted)
                {
                    _activeRunners.Remove(runner.InstanceId);
                    continue;
                }

                stepped++;
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

        public void EnqueueRunner(IFlowRunner runner)
        {
            if (runner != null && !runner.IsCompleted && !_readyRunners.Contains(runner))
                _readyRunners.Enqueue(runner);
        }
    }
}
