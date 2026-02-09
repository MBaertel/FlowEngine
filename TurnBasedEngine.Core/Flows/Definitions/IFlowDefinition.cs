using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TurnBasedEngine.Core.Flows.Graphs;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Definitions
{
    public interface IFlowDefinition
    {
        Guid Id { get; }
        string Name { get; }
        FlowGraph Flow { get; }
    }

    public interface IFlowDefinition<TIn,TOut> : IFlowDefinition
    {
        Type InputType { get; }
        Type OutputType { get; }
    }
}
