using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FlowEngine.Engine.Execution.Instances;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Execution
{
    public class FlowRunner<TResult> : IFlowRunner<TResult>
        where TResult : FlowValue
    {
        private TaskCompletionSource<TResult>? _completionSource;

        private bool _isWaiting;
        private bool _isCompleted;

        private IFlowStep _currentStep;
        private Guid _currentStepId;
        private FlowValue _currentInput;

        public bool IsWaiting => _isWaiting;
        public bool IsCompleted => _isCompleted;

        public IFlowInstance Flow { get; }
        public IFlowContext Context { get; }

        public FlowRunner(IFlowInstance instance,IFlowContext context)
        {
            Context = context;
            Flow = instance;

            _currentStepId = instance.StartStepId;
            _currentStep = instance.GetStep(_currentStepId);
            _currentInput = context.Payload;
        }

        public async Task StepAsync()
        {
            if (_isCompleted || _isWaiting) return;
            
            _isWaiting = true; 
            var stepResult = await _currentStep.ExecuteAsync(Context, _currentInput);
            _isWaiting = false;

            Context.Payload = stepResult;
            var next = Flow.ResolveNext(_currentStepId,Context);

            if(next == null)
            {
                _isCompleted = true;
                if (Context.Payload is not TResult typed)
                    throw new InvalidOperationException($"Flow completed with {Context.Payload.GetType()}, expected {typeof(TResult)}");
                _completionSource?.TrySetResult(typed);
                return;
            }
            else
            {
                _currentStep = next.Step;
                _currentInput = next.Input;
                _currentStepId = next.StepNodeId;
            }
        }

        public Task<TResult> WaitForCompletion()
        {
            if(IsCompleted)
            {
                if(Context.Payload is TResult typed)
                    return Task.FromResult(typed);
            }

            _completionSource ??= new TaskCompletionSource<TResult>();
            return _completionSource.Task;
        }

        Task<object?> IFlowRunner.WaitForCompletion() =>
            WaitForCompletion().ContinueWith(t => (object?) t.Result);
    }
}
