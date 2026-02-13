using FlowEngine.Engine.Flows.Execution;
using System.Runtime.CompilerServices;

namespace FlowEngine.Engine.Values
{
    public class FlowWait
    {
        public Guid FlowInstanceId { get; init; }

        protected IFlowRunner? _runner;

        public FlowWait(Guid flowInstaceId)
        {
            FlowInstanceId = flowInstaceId;
        }

        public void BindRunner(IFlowRunner runner)
        {
            _runner = runner; 
        }

        public Task<object?> AsTask()
        {
            return _runner.WaitForCompletion();
        }

        public TaskAwaiter<object?> GetAwaiter() => AsTask().GetAwaiter();
    }

    public class FlowWait<TResult> : FlowWait
    {
        public FlowWait(Guid instanceId)
            : base(instanceId) { }

        public void BindRunner(IFlowRunner<TResult> runner)
        {
            _runner = runner;
        }

        public new Task<TResult> AsTask()
        {
            if (_runner is IFlowRunner<TResult> typedRunner)
                return typedRunner.WaitForCompletion();

            throw new InvalidOperationException();
        }

        public TaskAwaiter<TResult> GetAwaiter() => AsTask().GetAwaiter();
    }
}
