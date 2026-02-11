using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

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

        public IFlowDefinition<TInput, TOutput> GetById<TInput, TOutput>(Guid id)
        {
            if (_definitionsById.TryGetValue(id, out var def))
            {
                if(def is IFlowDefinition<TInput, TOutput> typedDef)
                    return typedDef;
                throw new InvalidOperationException($"Flow with ID {id} is not of type {typeof(IFlowDefinition<TInput, TOutput>)}");
            }
            throw new InvalidOperationException($"No Flow with ID {id} was found.");
        }

        public IFlowDefinition<TInput, TOutput> GetByName<TInput, TOutput>(string name)
        {
            if (_definitionsByName.TryGetValue(name, out var def))
            {
                if(def is IFlowDefinition<TInput,TOutput> typedDef)
                    return typedDef;
                throw new InvalidOperationException($"Flow with Name {name} is not of type {typeof(IFlowDefinition<TInput, TOutput>)}");
            }
            throw new InvalidOperationException($"No Flow with Name {name} was found.");
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
