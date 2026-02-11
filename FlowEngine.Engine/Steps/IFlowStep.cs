using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Steps
{
    public interface IFlowStep
    {
        Task<object> ExecuteAsyncUntyped(IFlowContext ctx,object input);
        void UndoUntyped(object input,object output);
    }

    public interface IFlowStep<TInput,TOutput> : IFlowStep
    {
        new Task<TOutput> ExecuteAsync(IFlowContext ctx,TInput input);
        void Undo(TInput input,TOutput output);
    }
    
}
