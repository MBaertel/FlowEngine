using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Commands;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Orchestration
{
    public interface IFlowContext
    {
        public NodeValue Payload { get; set; }

        Task<NodeValue> ExecuteSubFlow(IFlowDefinition definition,NodeValue data);
        IFlowDefinition ResolveFlowDefinitionByName(string name);
        IFlowDefinition ResolveFlowDefinitionById(Guid id);

        void SetVar<T>(string name, T value);
        T GetVar<T>(string name);
    }
}
