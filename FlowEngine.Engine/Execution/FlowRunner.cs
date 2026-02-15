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
        private readonly IFlowOrchestrator _orchestrator;
        private readonly IFlowDefinitionRegistry _definitionRegistry;
        private TaskCompletionSource<TResult> _tcs;

        public IFlowInstance Instance { get; }
        public FlowStatus Status
        {
            get => Instance.Status;
            private set => Instance.Status = value;
        }

        public FlowRunner(IFlowOrchestrator orchestrator,IFlowDefinitionRegistry registry,IFlowInstance instance)
        {
            Instance = instance;
            _orchestrator = orchestrator;
            _definitionRegistry = registry;
        }

        public async Task StepAsync(CancellationToken ct = default)
        {
            if (Status == FlowStatus.Completed || Status == FlowStatus.Faulted || Status == FlowStatus.Waiting)
                return;
            
            if (Status == FlowStatus.Inactive) 
                Status = FlowStatus.Running;

            var step = Instance.GetStep(Instance.CurrentStepId);

            try
            {
                Status = FlowStatus.Waiting;
                var stepContext = new StepContext(Instance,_orchestrator,_definitionRegistry,Instance.CurrentStepId);
                var stepResult = await step.ExecuteAsyncUntyped(stepContext,Instance.Payload);
                Instance.Payload = stepResult;

                var frame = new HistoryFrame
                {
                    StepType = step.GetType(),
                    StepInstanceId = Instance.CurrentStepId,
                    StepOutput = stepResult,
                    RequiredUndoVariables = stepContext.UndoRequiredValues.ToDictionary()
                };
                Instance.History.Add(frame);

                Advance();
            }
            catch (Exception ex)
            {
                Instance.Status = FlowStatus.Faulted;
                throw;
            }
        }

        public Task UndoAsync(int steps = int.MaxValue, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> WaitForCompletion()
        {
            if (Instance.Status == FlowStatus.Completed)
                return Task.FromResult((TResult)Instance.Payload);

            _tcs ??= new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            return _tcs.Task;
        }

        async Task<object> IFlowRunner.WaitForCompletion()
        {
            return await WaitForCompletion();
        }

        private void Advance()
        {
            if(Instance.TryResolveNext(Instance.CurrentStepId,out var next))
            {
                Instance.CurrentStepId = next.StepId;
                Instance.Payload = next.Input;
                Instance.ActiveAwaiters.Clear();
                Status = FlowStatus.Running;
            }
            else
            {
                Status = FlowStatus.Completed;
                if (Instance.Payload is TResult typedPayload)
                    _tcs.TrySetResult(typedPayload);
                else
                    _tcs.SetException(new InvalidCastException($"Result was {Instance.Payload.GetType()}, expected {typeof(TResult)}"));
            }
        }
    }
}
