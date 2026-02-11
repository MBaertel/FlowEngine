using FlowEngine.Engine.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Tests.EngineTests.TestSteps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Tests.EngineTests.TestFlows
{
    public class MathTestFlow : FlowDefinitionBase<MathInput,float>
    {
        public override string Name => "MathTest";

        protected override FlowGraph BuildFlow()
        {
            var mathNode = new FlowNode(
                Guid.NewGuid(),
                typeof(MathStep));

            var nodes = new FlowNode[]
            {
                mathNode,
            };

            var edges = new FlowEdge[0];
            var transformers = new FlowTransformer[0];

            return new FlowGraph(mathNode.Id, nodes, edges, transformers);
        }
    }

    public class MathTestFlowMany : FlowDefinitionBase<MathInput,float>
    {
        public override string Name => "MathTestMany";

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
            for(int i = 0; i < 500; i++)
            {
                var node = new FlowNode(
                Guid.NewGuid(),
                typeof(MathStep));
                nodes.Add(node);

                if(lastNode != null)
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
