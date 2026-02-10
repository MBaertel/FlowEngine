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
        
    }
}
