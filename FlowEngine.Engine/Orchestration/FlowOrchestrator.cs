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

namespace FlowEngine.Engine.Flows.Orchestration
{
    public class FlowOrchestrator : IFlowOrchestrator
    {
        private readonly IFlowDefinitionRegistry _definitionRegistry;
        private readonly IStepFactory _stepFactory;

        private readonly ConcurrentDictionary<(Type input, Type output), Func<FlowOrchestrator, IFlowDefinition, object, Task<object>>> _typedMethodCache
            = new();

        private readonly List<IFlowRunner> _activeRunners = new();
        private readonly List<IFlowRunner> _runnersToStep = new();
        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _activeRunners.ToList().AsReadOnly();

        public FlowOrchestrator(IFlowDefinitionRegistry definitionRegistry,IStepFactory stepFactory)
        {
            _definitionRegistry = definitionRegistry;
            _stepFactory = stepFactory;
        }

        public async Task<object> ExecuteFlowUntypedAsync(
            IFlowDefinition flowDefinition,
            object input)
        {
            var inputType = flowDefinition.InputType;
            var outputType = flowDefinition.OutputType;

            // Find generic method definition by name
            var methodDef = typeof(FlowOrchestrator)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(m =>
                    m.Name == nameof(ExecuteFlowAsync) &&
                    m.IsGenericMethodDefinition &&
                    m.GetGenericArguments().Length == 2 &&
                    m.GetParameters().Length == 2
                );

            if (methodDef == null)
                throw new InvalidOperationException("Typed ExecuteFlowAsync method not found.");

            // Construct method with concrete types
            var typedMethod = methodDef.MakeGenericMethod(inputType, outputType);

            // Validate input type
            if (!inputType.IsInstanceOfType(input))
                throw new InvalidCastException($"Input is not of the expected type {inputType}");

            // Invoke and await dynamically
            var task = (Task)typedMethod.Invoke(this, new object[] { flowDefinition, input })!;
            await task.ConfigureAwait(false);

            // Extract Result property from Task<TResult>
            var resultProperty = task.GetType().GetProperty("Result")!;
            return resultProperty.GetValue(task)!;
        }

        //Input and Output
        public async Task<TResult> ExecuteFlowAsync<TInput, TResult>(
            IFlowDefinition<TInput, TResult> flow, 
            TInput input)
        {
            var context = new FlowContext(this, _definitionRegistry, input);
            var instance = new FlowInstance(_stepFactory,flow.Flow);
            var runner = new FlowRunner<TInput,TResult>(instance,context);

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

        public Task StepAllAsync()
        {
            _runnersToStep.Clear();
            _runnersToStep.AddRange(_activeRunners);

            foreach (var runner in _runnersToStep)
            {
                if(runner != null && !runner.IsWaiting)
                {
                    //_ = StepRunnerSafelyAsync(runner);
                    _ = StepRunnerSafelyAsync(runner);
                }
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
    }
}
