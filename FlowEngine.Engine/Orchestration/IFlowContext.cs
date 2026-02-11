using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Flows.Orchestration
{
    public interface IFlowContext
    {
        public object Payload { get; set; }

        FlowWait<TResult> ExecuteSubFlow<TInput,TResult>(IFlowDefinition<TInput,TResult> definition,TInput input);
        FlowWait ExecuteSubFlow(IFlowDefinition definition, object? input);

        IFlowDefinition? ResolveFlowDefinitionByName(string name);
        IFlowDefinition? ResolveFlowDefinitionById(Guid id);

        IFlowDefinition<TIn,TOut>? ResolveFlowDefinitionByName<TIn,TOut>(string name);
        IFlowDefinition<TIn, TOut>? ResolveFlowDefinitionById<TIn, TOut>(Guid id);

        void SetVar<T>(string name, T value);
        T GetVar<T>(string name);
    }
}
