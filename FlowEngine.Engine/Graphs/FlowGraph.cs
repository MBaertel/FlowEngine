using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs
{
    public sealed class FlowGraph
    {
        private Dictionary<Guid, FlowNode> _nodesById;
        private Dictionary<Guid, FlowEdge> _edgesById;
        private Dictionary<Guid,List<FlowEdge>> _edgesByNodeId;
        private Dictionary<Guid,FlowTransformer> _transformersByEdgeId;
        

        public Guid StartNodeId { get; }
        public IReadOnlyList<FlowNode> Steps => _nodesById.Values
            .ToList()
            .AsReadOnly();
        public IReadOnlyList<FlowEdge> Edges => _edgesByNodeId
            .SelectMany(x => x.Value)
            .ToList()
            .AsReadOnly();

        public IReadOnlyList<FlowTransformer> Transformers => _transformersByEdgeId.Values
            .ToList()
            .AsReadOnly();

        public FlowGraph(
            Guid startNodeId, 
            IEnumerable<FlowNode> nodes, 
            IEnumerable<FlowEdge> edges, 
            IEnumerable<FlowTransformer> transformers)
        {
            StartNodeId = startNodeId;
            _nodesById = nodes.ToDictionary(n => n.Id, n => n);
            _edgesById = edges.ToDictionary(e => e.Id, e => e);
            
            _edgesByNodeId = edges.GroupBy(e => e.FromNodeId)
                .ToDictionary(g => g.Key, g => g.ToList());

            _transformersByEdgeId = transformers.ToDictionary(t => t.EdgeId, t => t);

            foreach (var node in nodes)
            {
                if (!_edgesByNodeId.ContainsKey(node.Id))
                    _edgesByNodeId[node.Id] = new List<FlowEdge>();
            }

            Validate();
        }


        public FlowEdge? GetNextEdge(Guid currentId,IFlowContext context)
        {
            var transitions = _edgesByNodeId[currentId];
            foreach (var transition in transitions)
            {
                if (transition.CanTransition(context,context.Payload))
                    return transition;
            }
            return null;
        }

        public FlowTransformer? GetTransformer(Guid edgeId)
        {
            if(_transformersByEdgeId.TryGetValue(edgeId, out var transformer))
                return transformer;
            return null;
        }

        public FlowNode GetStep(Guid nodeId)
        {
            if (_nodesById.TryGetValue(nodeId, out var node))
                return node;
            throw new InvalidOperationException();
        }

        private void Validate()
        {
            List<string> errors = new List<string>();

            if (!_nodesById.ContainsKey(StartNodeId))
                errors.Add("Start Node does not exist.");

            foreach(var edge in _edgesById.Values)
            {
                if (!_nodesById.ContainsKey(edge.FromNodeId))
                    errors.Add($"Edge {edge.Id} From Node does not exist.");

                if(!_nodesById.ContainsKey(edge.ToNodeId))
                    errors.Add($"Edge {edge.Id} To Node does not exist.");
            }

            foreach(var transformer in _transformersByEdgeId.Values)
            {
                if (!_edgesById.ContainsKey(transformer.EdgeId))
                    errors.Add($"Transformer references missing edge {transformer.EdgeId}");
            }

            if(errors.Any())
                throw new InvalidFlowException(errors.ToArray());
        }
    }
}
