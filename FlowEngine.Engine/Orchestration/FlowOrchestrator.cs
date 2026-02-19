using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Steps;
using FlowEngine.Engine.Execution;
using FlowEngine.Engine.Execution.Context;
using System.Collections.Concurrent;
using FlowEngine.Engine.Values;
using System.Security.Cryptography.X509Certificates;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowOrchestrator : IFlowOrchestrator
    {
        private readonly IFlowDefinitionRegistry _definitionRegistry;
        private readonly IStepFactory _stepFactory;

        private readonly Dictionary<Guid, IFlowRunner> _runnersById = new();
        private readonly List<IFlowRunner> _activeRunners = new();
        private readonly ConcurrentQueue<Exception> _faults = new();

        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _runnersById.Values.ToList().AsReadOnly();

        public FlowOrchestrator(IFlowDefinitionRegistry definitionRegistry,IStepFactory stepFactory)
        {
            _definitionRegistry = definitionRegistry;
            _stepFactory = stepFactory;
        }

        public IFlowRunner AddFlow(IFlowDefinition flow, object input)
        {
            var instance = new FlowInstance(flow, _stepFactory,input);
            var runner = new FlowRunner<object>(this,_definitionRegistry,instance);

            _runnersById.Add(instance.InstanceId,runner);
            _activeRunners.Add(runner);
            return runner;
        }

        public IFlowRunner<TResult> AddFlow<TInput, TResult>(IFlowDefinition<TInput, TResult> flow, TInput input)
        {
            var instance = new FlowInstance(flow, _stepFactory,input);
            var context = new FlowContext(instance);
            var runner = new FlowRunner<TResult>(this, _definitionRegistry, instance);

            _runnersById.Add(instance.InstanceId,runner);
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

        public Task StepAllAsync()
        {
            for (int i = 0; i < _activeRunners.Count; i++)
            {
                var runner = _activeRunners[i];
                if (runner != null && (runner.Status == FlowStatus.Running || runner.Status == FlowStatus.Inactive))
                {
                    var stepTask = runner.StepAsync();
                    Observe(stepTask);
                }
            }

            if (_faults.TryDequeue(out var ex))
                throw ex;

            return Task.CompletedTask;
        }

        private void Observe(Task task)
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                    _faults.Enqueue(task.Exception);
                return;
            }

            task.ContinueWith(t =>
            {
                _faults.Enqueue(t.Exception);
            },
            TaskContinuationOptions.OnlyOnFaulted);
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
