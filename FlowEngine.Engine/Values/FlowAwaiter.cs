using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Orchestration;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace FlowEngine.Engine.Values
{
    public class FlowAwaiterBase
    {
        public Guid SubflowInstanceId { get; set; }
        public bool IsCompleted { get; set; }

        private IFlowRunner _runner;

        protected FlowAwaiterBase(Guid instanceId)
        {
            SubflowInstanceId = instanceId;
        }

        public void ConnectRunner(IFlowRunner runner)
        {
            _runner = runner;
        }

        public Task<object> AsTask()
        {
           return _runner.WaitForCompletion();
        }
    }

    public class FlowAwaiter<TResult> : FlowAwaiterBase
    {
        public Guid SubflowInstanceId { get; set; }
        public bool IsCompleted { get; private set; } = false;
        public TResult? Result { get; private set; } = default;

        private IFlowRunner<TResult> _runner;

        public FlowAwaiter(Guid SubflowInstanceId)
            : base(SubflowInstanceId)
        { }

        public FlowAwaiter(IFlowRunner<TResult> runner)
            : base(runner.Instance.InstanceId)
        {
            _runner = runner;
            if(_runner.Instance.Status == FlowStatus.Completed)
            {
                Result = (TResult)_runner.Instance.Payload;
                IsCompleted = true;
            }
        }

        public Task<TResult> AsTask()
        {
            if(IsCompleted)
                return Task.FromResult(Result);

            if (_runner == null)
                throw new InvalidOperationException("Runner not found");

            return _runner.WaitForCompletion();
        }
    } 
}
