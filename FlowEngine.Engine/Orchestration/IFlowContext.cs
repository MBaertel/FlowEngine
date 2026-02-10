using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public interface IFlowContext
    {
        public FlowValue Payload { get; set; }

        Task<FlowValue> ExecuteSubFlow(IFlowDefinition definition,FlowValue data);
        IFlowDefinition? ResolveFlowDefinitionByName(string name);
        IFlowDefinition? ResolveFlowDefinitionById(Guid id);

        void SetVar<T>(string name, T value);
        T GetVar<T>(string name);
    }
}
