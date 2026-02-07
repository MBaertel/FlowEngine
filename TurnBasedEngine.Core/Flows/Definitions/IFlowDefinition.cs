using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Definitions
{
    public interface IFlowDefinition
    {
        Guid Id { get; }
        string Name { get; }

        Guid StartNodeId { get; }

        IReadOnlyList<FlowNodeDescriptor> Nodes { get; }

        IReadOnlyList<FlowTransitionDescriptor> Transitions { get; }
        
    }

    public class FlowDefinition : IFlowDefinition
    {
        private Dictionary<Guid, FlowNodeDescriptor> _nodesById;
        private Dictionary<Guid, List<FlowTransitionDescriptor>> _transitionsByNode;

        public Guid Id { get; }
        public string Name { get; }
        public Guid StartNodeId { get; }

        public IReadOnlyList<FlowNodeDescriptor> Nodes { get; }
        public IReadOnlyList<FlowTransitionDescriptor> Transitions { get; }

        public FlowDefinition(
            Guid id,
            string name,
            Guid startNodeId,
            IEnumerable<FlowNodeDescriptor> nodes,
            IEnumerable<FlowTransitionDescriptor> transitions)
        {
            Id = id;
            Name = name;
            StartNodeId = startNodeId;

            Nodes = Nodes.ToList().AsReadOnly();
            Transitions = Transitions.ToList().AsReadOnly();

            _nodesById = Nodes
                .ToDictionary(n => n.Id);
            
            _transitionsByNode = Transitions
                .GroupBy(t => t.FromNode)
                .ToDictionary(g => g.Key, g => g.ToList());

            Validate();
        }

        private void Validate()
        {
            if (!_nodesById.ContainsKey(StartNodeId))
                throw new InvalidOperationException("Start Node is Missing from Nodes.");

            foreach (var transition in Transitions)
            {
                if (!_nodesById.ContainsKey(transition.FromNode))
                    throw new InvalidOperationException($"Transition FromNode {transition.FromNode} does not exist");
            }
        }
    }

    public class FlowNodeDescriptor
    {
        private readonly Func<IServiceProvider, IFlowStep> _factory;

        public Guid Id { get; }
        public string Name { get; }

        public FlowNodeDescriptor(Guid id, string name, Func<IServiceProvider, IFlowStep> factory)
        {
            Id = id;
            Name = name;
            _factory = factory;
        }

        public IFlowStep CreateStep(IServiceProvider sp)
            => _factory(sp);
    }

    public class FlowTransitionDescriptor
    {
        private readonly Func<IServiceProvider,IFlowTransition> _factory;
        public Guid FromNode { get; }

        public FlowTransitionDescriptor(Guid fromNode,Func<IServiceProvider,IFlowTransition> factory)
        {
            FromNode = fromNode;
            _factory = factory;
        }

        public IFlowTransition Create(IServiceProvider sp)
            => _factory(sp);
    }
}
