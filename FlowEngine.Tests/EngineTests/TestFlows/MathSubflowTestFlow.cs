using FlowEngine.Engine.Definitions;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Tests.EngineTests.TestSteps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Tests.EngineTests.TestFlows
{
    internal class MathSubflowTestFlow : FlowDefinitionBase<MathInput, float>
    {
        public override string Name => "MathTestSubflow";

        protected override FlowGraph BuildFlow()
        {
            var mathNode = new FlowNode(
                Guid.NewGuid(),
                typeof(TestSubflowStep));

            var nodes = new FlowNode[]
            {
                mathNode,
            };

            var edges = new FlowEdge[0];
            var transformers = new FlowTransformer[0];

            return new FlowGraph(mathNode.Id, nodes, edges, transformers);
        }
    }

    internal class MathSubflowTwiceTestFlow : FlowDefinitionBase<MathInput, float>
    {
        public override string Name => "MathTestSubflowTwice";

        protected override FlowGraph BuildFlow()
        {
            var mathNode = new FlowNode(
                Guid.NewGuid(),
                typeof(TestSubflowStep));

            var mathNode2 = new FlowNode(
                Guid.NewGuid(),
                typeof(TestSubflowStep));

            var nodes = new FlowNode[]
            {
                mathNode,
                mathNode2
            };

            var edge = new FlowEdge(Guid.NewGuid(), mathNode.Id, mathNode2.Id);
            var edges = new FlowEdge[]
            {
                edge
            };
            var transformers = new FlowTransformer[]
            {
                new FlowTransformer(edge.Id,(x,y) => new MathInput((float)y,1,MathModes.Add))
            };

            return new FlowGraph(mathNode.Id, nodes, edges, transformers);
        }
    }

    public class MathSubflowTestMany : FlowDefinitionBase<MathInput, float>
    {
        public override string Name => "MathSubflowTestMany";

        protected override FlowGraph BuildFlow()
        {
            var nodes = new List<FlowNode>();
            var edges = new List<FlowEdge>();
            var transformers = new List<FlowTransformer>();
            Func<IFlowContext, object, object> lambda = (ctx, payload) =>
            {
                var res = (float)payload;
                var mathIn = new MathInput(res, 1, MathModes.Add);
                return mathIn;
            };

            FlowNode lastNode = null;
            for (int i = 0; i < 500; i++)
            {
                var node = new FlowNode(
                Guid.NewGuid(),
                typeof(TestSubflowStep));
                nodes.Add(node);

                if (lastNode != null)
                {
                    var edge = new FlowEdge(
                        Guid.NewGuid(),
                        lastNode.Id,
                        node.Id,
                        null);
                    edges.Add(edge);

                    var transformer = new FlowTransformer(
                        edge.Id,
                        lambda);
                    transformers.Add(transformer);
                }
                lastNode = node;
            }

            return new FlowGraph(nodes.First().Id, nodes, edges, transformers);
        }
    }
}
