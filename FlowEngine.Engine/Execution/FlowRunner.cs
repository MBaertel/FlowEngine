using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Execution.Instances;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Flows.Execution
{
    public class FlowRunner<TResult> : IFlowRunner<TResult>
    {
        private TaskCompletionSource<TResult>? _tcs = 
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        private bool _isWaiting = false;
        private bool _isCompleted = false;
        private bool _isFinished = false;

        private IFlowStep _currentStep;
        private Guid _currentStepId;

        public bool IsWaiting => _isWaiting;
        public bool IsCompleted => _isCompleted;

        public IFlowInstance Instance { get; }
        public IFlowContext Context { get; }
        public Guid RunnerId { get; }

        public FlowRunner(IFlowInstance instance,IFlowContext context)
        {
            RunnerId = instance.InstanceId;
            Context = context;
            Instance = instance;

            _currentStepId = instance.StartStepId;
            _currentStep = instance.GetStep(_currentStepId);
        }

        public async ValueTask StepAsync()
        {
            if (_isFinished || _isWaiting) return;
            
            _isWaiting = true;
            var stepResult = await _currentStep.ExecuteAsyncUntyped(Context, Context.Payload);
            _isWaiting = false;

            Context.Payload = stepResult;

            var next = Instance.ResolveNext(_currentStepId, Context);

            if (next == null)
            {
                _isFinished = true;
                if (Context.Payload is not TResult typed)
                    throw new InvalidOperationException($"Flow completed with {Context.Payload.GetType()}, expected {typeof(TResult)}");
                _tcs?.TrySetResult(typed);
                return;
            }
            else
            {
                _currentStep = next.Step;
                _currentStepId = next.StepNodeId;
                Context.Payload = next.Input;
            }
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

        public Guid GetCurrentStepId()
        {
            return _currentStepId; 
        }

        Task<object?> IFlowRunner.WaitForCompletion() =>
            WaitForCompletion().ContinueWith(t => (object?) t.Result);
    }
}
