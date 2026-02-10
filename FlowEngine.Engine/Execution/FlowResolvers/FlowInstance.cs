using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution.Instances
{
    public class FlowInstance : IFlowInstance
    {
        private readonly FlowGraph _graph;
        private readonly IStepFactory _stepFactory;

        public FlowInstance(IStepFactory stepFactory,FlowGraph graph)
        {
            _stepFactory = stepFactory;
            _graph = graph;
        }

        public FlowGraph Graph => _graph;
        public Guid StartStepId => _graph.StartNodeId;

        public IFlowStep GetStep(Guid stepId)
        {
            var step = _graph.GetStep(stepId);
            return _stepFactory.Resolve(step.StepType);
        }

        public NextStepResult? ResolveNext(Guid currentStepId, IFlowContext ctx)
        {
            var edge = _graph.GetNextEdge(currentStepId, ctx);
            if (edge == null) return null;
            
            var flowNode = _graph.GetStep(edge.ToNodeId);
            var transformer = _graph.GetTransformer(edge.Id);
            var step = _stepFactory.Resolve(flowNode.StepType);

            var payload = transformer != null ? transformer.Transform(ctx, ctx.Payload) : ctx.Payload;

            return new NextStepResult(flowNode.Id, step, payload);
        }
    }
}
