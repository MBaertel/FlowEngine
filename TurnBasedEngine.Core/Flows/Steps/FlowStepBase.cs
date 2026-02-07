using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;

namespace TurnBasedEngine.Core.Flows.Steps
{
    public abstract class FlowStepBase<TInput, TOutput>
        : IFlowStep<TInput, TOutput>
        where TInput : StepValue
        where TOutput : StepValue
    {
        public Guid Id { get; } = Guid.NewGuid();

        public abstract Task<TOutput> ExecuteAsync(FlowContext flowContext, TInput input);

        public abstract void Undo(FlowContext flowContext,TInput input,TOutput output);

        public async Task<StepValue> ExecuteAsync(FlowContext flowContext, StepValue input)
        {
            if (input is TInput tInput)
                return await ExecuteAsync(flowContext, tInput);
            throw new ArgumentException($"input is not {typeof(TInput)}");
        }

        public void Undo(FlowContext flowContext, StepValue input, StepValue output)
        {
            if(input is TInput tInput && output is TOutput tOutput)
                Undo(flowContext, tInput, tOutput);
            throw new ArgumentException($"input/output type did not match {typeof(TInput)} or {typeof(TOutput)}");
        }
    }
}
