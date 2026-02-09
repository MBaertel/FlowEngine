using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Flows.Graphs
{
    public sealed class FlowGraph
    {
        private Dictionary<Guid, FlowNode> _nodesById = new();
        private Dictionary<Guid, FlowEdge> _edgesById = new();
        private Dictionary<Guid,List<FlowEdge>> _edgesByNodeId = new();
        private Dictionary<Guid,FlowTransformer> _transformersByEdgeId = new();
        

        public Guid StartNodeId { get; }
        public IReadOnlyList<FlowNode> Steps => _nodesById.Values
            .ToList()
            .AsReadOnly();
        public IReadOnlyList<FlowEdge> Edges => _edgesByNodeId
            .SelectMany(x => x.Value)
            .ToList()
            .AsReadOnly();

        private FlowEdge? GetNextEdge(Guid currentId,IFlowContext context)
        {
            var transitions = _edgesByNodeId[currentId];
            foreach (var transition in transitions)
            {
                if (transition.CanTransition(context))
                    return transition;
            }
            return null;
        }

        private IFlowStep? GetStep(Guid nodeId)
        {
            if (_nodesById.TryGetValue(nodeId, out var node))
                return node.Step;
            return null;
        }

        private FlowTransformer? GetTransformer(Guid edgeId)
        {
            if (_transformersByEdgeId.TryGetValue(edgeId, out var transformer))
                return transformer;
            return null;
        }

        public (IFlowStep? step,FlowTransformer? transformer) GetNextStep(Guid currentStepId,IFlowContext ctx)
        {
            var edge = GetNextEdge(currentStepId, ctx);
            var step = GetStep(edge.ToNodeId);
            var transformer = GetTransformer(edge.Id);

            return (step, transformer);
        }

        

        public IFlowStep GetStartStep()
        {
            return _nodesById[StartNodeId].Step;
        }
    }
}
