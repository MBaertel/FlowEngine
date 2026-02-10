using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Steps
{
    public interface IFlowStep
    {
        Guid Id { get; }
        Task<FlowValue> ExecuteAsync(IFlowContext ctx,FlowValue input);
        void Undo(FlowValue input,FlowValue output);
    }

    public interface IFlowStep<TInput,TOutput> : IFlowStep
    {
        new Task<FlowValue> ExecuteAsync(IFlowContext ctx,FlowValue input);
    }
    
}
