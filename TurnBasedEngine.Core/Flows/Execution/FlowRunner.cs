using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Graphs;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Execution
{
    public class FlowRunner<TResult> : IFlowRunner<TResult>
        where TResult : NodeValue
    {
        private TaskCompletionSource<TResult>? _completionSource;

        private bool _isWaiting;
        private bool _isCompleted;
        private IFlowStep _currentStep;
        private FlowTransformer? _currentTransformer;

        public bool IsWaiting => _isWaiting;
        public bool IsCompleted => _isCompleted;

        public FlowGraph Flow { get; }
        public IFlowContext Context { get; }

        public FlowRunner(IFlowContext context,FlowGraph graph)
        {
            Context = context;
            Flow = graph;

            _currentStep = graph.GetStartStep();
        }

        public async Task StepAsync()
        {
            if (_isCompleted || _isWaiting) return;
            
            _isWaiting = true;
            var input = _currentTransformer != null ? _currentTransformer.Transform(Context) : Context.Payload; 
            var stepResult = await _currentStep.ExecuteAsync(Context, input);
            _isWaiting = false;

            Context.Payload = stepResult;
            var next = Flow.GetNextStep(_currentStep.Id, Context);
            if(next.step == null)
            {
                _isCompleted = true;
                if (Context.Payload is not TResult typed)
                    throw new InvalidOperationException($"Flow completed with {Context.Payload.GetType()}, expected {typeof(TResult)}");
                _completionSource?.TrySetResult(typed);
                return;
            }
            else
            {
                _currentStep = next.step;
                _currentTransformer = next.transformer;
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
