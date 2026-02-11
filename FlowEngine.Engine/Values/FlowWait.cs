using FlowEngine.Engine.Execution.Instances;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Orchestration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlowEngine.Engine.Values
{
    public class FlowWait
    {
        public Guid FlowInstanceId { get; init; }

        private IFlowRunner? _runner;

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
        protected IFlowRunner<TResult>? _runner;

        public FlowWait(Guid instanceId)
            : base(instanceId) { }

        public void BindRunner(IFlowRunner<TResult> runner)
        {
            _runner = runner;
        }

        public new Task<TResult> AsTask()
        {
            return _runner.WaitForCompletion();
        }

        public TaskAwaiter<TResult> GetAwaiter() => AsTask().GetAwaiter();
    }
}
