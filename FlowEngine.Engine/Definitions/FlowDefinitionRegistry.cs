using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Flows.Definitions
{
    public class FlowDefinitionRegistry : IFlowDefinitionRegistry
    {
        private readonly Dictionary<Guid, IFlowDefinition> _definitionsById = new();
        private readonly Dictionary<string, IFlowDefinition> _definitionsByName = new();

        public FlowDefinitionRegistry(IServiceProvider serviceProvider) 
        {
            var initialDefinitions = serviceProvider.GetServices<IFlowDefinition>();
            foreach (var def in initialDefinitions)
            {
                Register(def);
            }
        }

        public IFlowDefinition GetById(Guid id)
        {
            if (_definitionsById.TryGetValue(id, out var def))
                return def;
            throw new InvalidOperationException($"No Flow with ID {id} was found.");
        }

        public IFlowDefinition GetByName(string name)
        {
            if(_definitionsByName.TryGetValue(name, out var def))
                return def;
            throw new InvalidOperationException($"No Flow with Name {name} was found.");
        }

        public void Register(IFlowDefinition definition)
        {
            if (!_definitionsById.TryAdd(definition.Id, definition))
                throw new InvalidOperationException($"Flow with id: {definition.Id} has already been registered.");
            if(!_definitionsByName.TryAdd(definition.Name, definition))
                throw new InvalidOperationException($"Flow with name: {definition.Name} has already been registered.");
        }
    }
}
