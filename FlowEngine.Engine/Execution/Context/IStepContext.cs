using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution.Context
{
    public interface IStepContext
    {
        IDictionary<string, object?> UndoRequiredValues { get; }

        IFlowDefinition GetFlowById(Guid id);
        IFlowDefinition GetFlowByName(string name);

        IFlowDefinition<TIn,TOut> GetFlowById<TIn,TOut>(Guid id);
        IFlowDefinition<TIn,TOut> GetFlowByName<TIn,TOut>(string name);

        Task<object> ExecuteSubflow(IFlowDefinition flowDefinition,object input);
        Task<TOut> ExecuteSubFlow<TIn, TOut>(IFlowDefinition<TIn, TOut> flowDefinition, TIn input);
    }
}
