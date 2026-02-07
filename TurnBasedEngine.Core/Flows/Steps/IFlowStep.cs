using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;

namespace TurnBasedEngine.Core.Flows.Steps
{
    public interface IFlowStep
    {
        Guid Id { get; }
        Task<StepValue> ExecuteAsync(FlowContext flowContext,StepValue input);
        void Undo(FlowContext flowContext,StepValue input,StepValue output);
    }

    public interface IFlowStep<TInput,TOutput> : IFlowStep
        where TInput : StepValue
        where TOutput : StepValue
    {
        Task<TOutput> ExecuteAsync(FlowContext flowContext, TInput input);
        void Undo(FlowContext flowContext,TInput input,TOutput output);
    }


}
