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
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Flows.Execution
{
    public class FlowRunner<TIn,TResult> : IFlowRunner<TIn,TResult>
    {
        private TaskCompletionSource<TResult>? _tcs = 
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        private bool _isWaiting;
        private bool _isCompleted;
        private FlowWait _currentWait;

        private IFlowStep _currentStep;
        private Guid _currentStepId;

        public bool IsWaiting => _isWaiting;
        public bool IsCompleted => _isCompleted;

        public IFlowInstance Flow { get; }
        public IFlowContext Context { get; }
        public Guid Id { get; }

        public FlowRunner(IFlowInstance instance,IFlowContext context)
        {
            Context = context;
            Flow = instance;

            _currentStepId = instance.StartStepId;
            _currentStep = instance.GetStep(_currentStepId);
        }

        public async ValueTask StepAsync()
        {
            if (_isCompleted || _isWaiting) return;
            
            _isWaiting = true;
            var stepResult = await _currentStep.ExecuteAsyncUntyped(Context, Context.Payload);

            if(stepResult is FlowWait wait)
            {
                _currentWait = wait;
                stepResult = await wait.WaitForCompletion();
            }
            _isWaiting = false;

            Context.Payload = stepResult;

            var next = Flow.ResolveNext(_currentStepId, Context);

            if (next == null)
            {
                _isCompleted = true;
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
            if (_isCompleted)
                return Task.FromResult((TResult)Context.Payload!);

            _tcs ??= new TaskCompletionSource<TResult>();
            return _tcs.Task;
        }

        Task<object?> IFlowRunner.WaitForCompletion() =>
            WaitForCompletion().ContinueWith(t => (object?) t.Result);

        private async Task ResumeAfterSubflow(FlowWait wait)
        {
            try
            {
                var stepResult = await wait.WaitForCompletion();
                _currentWait = null;
                _isWaiting = false;
                Advance(stepResult);
            }
            catch (Exception e)
            {
                _isCompleted = true;
                _tcs?.TrySetException(e);
            }
        }

        private void Advance(object stepResult)
        {

        }
    }
}
