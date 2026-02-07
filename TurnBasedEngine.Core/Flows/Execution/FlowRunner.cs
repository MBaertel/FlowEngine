using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Instances;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Execution
{
    public class FlowRunner : IFlowRunner
    {
        private bool _isWaiting;
        private TaskCompletionSource<StepValue>? _completionTcs;
        private readonly IFlowInstance _flow;

        public bool IsWaiting => _isWaiting;
        public IFlowInstance Flow => _flow;

        public FlowRunner(IFlowInstance instance)
        {
            _flow = instance;
        }

        public Task<StepValue> WaitForCompletion()
        {
            if(_flow.IsCompleted)
                return Task.FromResult(_flow.Payload);

            _completionTcs ??= new TaskCompletionSource<StepValue>();
            return _completionTcs.Task;
        }

        public async Task StepAsync()
        {
            if (_isWaiting || _flow.IsCompleted) return;

            _isWaiting = true;

            try
            {
                await _flow.StepAsync();
                if (_flow.IsCompleted && _completionTcs != null)
                    _completionTcs.SetResult(_flow.Payload);
            }
            finally
            {
                _isWaiting = false;
            }
        }
    }
}
