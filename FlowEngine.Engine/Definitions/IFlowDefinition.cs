using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;

namespace FlowEngine.Engine.Flows.Definitions
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
