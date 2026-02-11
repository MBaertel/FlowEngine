using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public interface IFlowContext
    {
        public object Payload { get; set; }

        Task<TResult> ExecuteSubFlow<TInput,TResult>(IFlowDefinition<TInput,TResult> definition,TInput input);
        Task<object> ExecuteSubFlow(IFlowDefinition definition, object? input);

        IFlowDefinition? ResolveFlowDefinitionByName(string name);
        IFlowDefinition? ResolveFlowDefinitionById(Guid id);

        void SetVar<T>(string name, T value);
        T GetVar<T>(string name);
    }
}
