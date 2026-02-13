using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FlowEngine.Engine.Execution;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Flows.Execution
{
    public class FlowRunner<TResult> : IFlowRunner<TResult>
    {
        private TaskCompletionSource<TResult>? _tcs = 
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        private readonly IFlowDefinitionRegistry _definitionRegistry;
        private readonly IFlowOrchestrator _orchestrator;

        private bool _isWaiting = false;
        private bool _isCompleted = false;
        private bool _isFinished = false;

        private IFlowStep _currentStep;
        private Guid _currentStepId;

        public bool IsWaiting => _isWaiting;
        public bool IsCompleted => _isCompleted;

        public IFlowInstance Instance { get; }
        public IFlowContext Context { get; }
        public Guid InstanceId { get; }


        public FlowRunner(IFlowInstance instance, IFlowContext context, IFlowOrchestrator orchestrator, IFlowDefinitionRegistry registry)
        {
            _orchestrator = orchestrator;
            _definitionRegistry = registry;

            InstanceId = instance.InstanceId;
            Context = context;
            Instance = instance;

            _currentStepId = instance.StartStepId;
            _currentStep = instance.GetStep(_currentStepId);
        }

        public Task StepAsync()
        {
            if (_isFinished || _isWaiting)
                return Task.CompletedTask;

            var stepContext = new StepContext(Instance, _orchestrator, _definitionRegistry, _currentStepId);
            var stepTask = _currentStep.ExecuteAsyncUntyped(stepContext, Instance.Payload);

            if (!stepTask.IsCompleted)
            {
                _isWaiting = true;
                stepTask.ContinueWith(OnStepComplete, TaskContinuationOptions.ExecuteSynchronously);
            }
            else
            {
                _isWaiting = false;
                var stepResult = stepTask.Result;
                Advance(stepResult);
            }

            return Task.CompletedTask;
        }

        private void Advance(object stepResult)
        {
            _isWaiting = false;
            Instance.Payload = stepResult;
            if (Instance.TryResolveNext(_currentStepId, Context, out var next))
            {
                _currentStepId = next.StepNodeId;
                _currentStep = Instance.GetStep(_currentStepId);
                Instance.Payload = next.Input;
                _orchestrator.EnqueueRunner(this);
            }
            else
            {
                _isFinished = true;
                if (Context.Payload is not TResult typed)
                    throw new InvalidOperationException($"Flow completed with {Context.Payload.GetType()}, expected {typeof(TResult)}");
                _tcs?.TrySetResult(typed);
                return;
            }
        }

        private void OnStepComplete(Task<object> task)
        {
            Advance(task.Result);
        }
            

        public Task<TResult> WaitForCompletion()
        {
            if (_isFinished)
            {
                _isCompleted = true;
                return Task.FromResult((TResult)Context.Payload!);
            }

            _tcs ??= new TaskCompletionSource<TResult>();
            return _tcs.Task.ContinueWith(t =>
            {
                _isCompleted = true;
                return t.Result;
            });
        }

        Task<object?> IFlowRunner.WaitForCompletion() =>
            WaitForCompletion().ContinueWith(t => (object?) t.Result);
    }
}
