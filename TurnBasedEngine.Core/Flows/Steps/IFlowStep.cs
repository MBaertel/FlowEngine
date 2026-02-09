using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Steps
{
    public interface IFlowStep
    {
        Guid Id { get; }
        Task<NodeValue> ExecuteAsync(IFlowContext ctx,NodeValue input);
        void Undo(NodeValue input,NodeValue output);
    }

    public interface IFlowStep<TInput,TOutput> : IFlowStep
    {
        Task<TOutput> ExecuteAsync(IFlowContext ctx,NodeValue input);
    }
    
}
