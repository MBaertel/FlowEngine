using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;

namespace FlowEngine.Engine.Flows.Graphs.Builder
{
    public static class FlowBuilder
    {
        public static FlowBuilder<TOut> StartWith<TIn,TOut>(IFlowStep<TIn,TOut> step)
        {
            return new FlowBuilder<TOut>(step);
        }
    }

    public sealed class FlowBuilder<TCurrent>
    {
        private readonly List<FlowNode> _nodes = new();
        private readonly List<FlowEdge> _edges = new();
        private readonly List<FlowTransformer> _transformers = new();

        private FlowNode _current;

        internal FlowBuilder(Type step)
        {
            _current = AddNode(step);
        }

        private FlowBuilder(List<FlowNode> nodes, List<FlowEdge> edges,List<FlowTransformer> transformers, FlowNode current)
        {
            _nodes = nodes;
            _edges = edges;
            _transformers = transformers;
            _current = current;
        }

        private FlowNode AddNode(Type step)
        {
            var nodeId = Guid.NewGuid();
            var node = new FlowNode(nodeId, step);
            _nodes.Add(node);
            return node;
        }

        public FlowBuilder<TNextOut> Then<TNextOut>(
            IFlowStep<TCurrent,TNextOut> step,
            Func<IFlowContext,TCurrent,bool>? predicate = null)
        {
            var nextNode = AddNode(step);

            var condition = predicate != null ?
                FlowLambda.When(predicate) :
                null;

            var edgeId = Guid.NewGuid();
            _edges.Add(new FlowEdge(
                edgeId, 
                _current.Id, 
                nextNode.Id,
                condition));

            _current = nextNode;

            return new FlowBuilder<TNextOut>(
                _nodes, _edges, _transformers, _current);
        }

        public FlowBuilder<TNextOut> Then<TNextIn,TNextOut>(
            IFlowStep<TNextIn,TNextOut> step,
            Func<IFlowContext,TCurrent,TNextIn> map,
            Func<IFlowContext, TCurrent, bool>? predicate = null)
        {
            var nextNode = AddNode(step);

            var condition = predicate != null ?
                FlowLambda.When(predicate) :
                null;

            var edgeId = Guid.NewGuid();
            _edges.Add(new FlowEdge(
                edgeId, 
                _current.Id, 
                nextNode.Id,
                condition));
            
            _transformers.Add(new FlowTransformer(
                edgeId, FlowLambda.Transform(map)));
            
            _current = nextNode;

            return new FlowBuilder<TNextOut>(
                _nodes,_edges, _transformers, _current);
        }

        public FlowGraph Build()
        {
            return new FlowGraph(
                _nodes[0].Id,
                _nodes,
                _edges,
                _transformers);
        }
    }
}
