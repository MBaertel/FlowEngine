using FlowEngine.Engine.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Tests.EngineTests.TestSteps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Tests.EngineTests.TestFlows
{
    public class MathTestFlow : FlowDefinitionBase
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

    public class MathTestFlowMult : FlowDefinitionBase
    {
        public override string Name => "MathTestMult";

        protected override FlowGraph BuildFlow()
        {
            var mathNode = new FlowNode(
                Guid.NewGuid(),
                typeof(MathStep));
            var mathNode2 = new FlowNode(
                Guid.NewGuid(),
                typeof(MathStep));

            var nodes = new FlowNode[]
            {
                mathNode,
                mathNode2,
            };

            var edge = new FlowEdge(Guid.NewGuid(), mathNode.Id, mathNode2.Id, null);
            var edges = new FlowEdge[]
            {
                edge
            };

            var transformers = new FlowTransformer[]
            {
                new FlowTransformer(edge.Id,(ctx,payload) =>
                {
                    var res = payload.Unwrap<float>();
                    var mathIn = new MathInput(res,2,MathModes.Multiply);
                    return FlowValue.Wrap(mathIn);
                })
            };

            return new FlowGraph(mathNode.Id, nodes, edges, transformers);
        }
    }
}
