using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Flows.Definitions
{
    public interface IFlowDefinitionRegistry
    {
        void Register(IFlowDefinition definition);
        IFlowDefinition GetByName(string name);
        IFlowDefinition GetById(Guid id);

        IFlowDefinition<TInput,TOutput> GetByName<TInput,TOutput>(string name);
        IFlowDefinition<TInput,TOutput> GetById<TInput,TOutput>(Guid id);
        
    }
}
