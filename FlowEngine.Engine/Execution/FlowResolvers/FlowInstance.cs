using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Steps;
using FlowEngine.Engine.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution.Instances
{
    public class FlowInstance : IFlowInstance
    {
        public Guid InstanceId { get; init; } = Guid.NewGuid();
        public IFlowDefinition Definition { get; init; }

        public Guid StartStepId => _graph.StartNodeId;
        public Guid CurrentStepId { get; set; }
        
        public object Payload { get; set; }
        public IDictionary<string, object?> Variables { get; } = new Dictionary<string, object?>();


        private readonly Dictionary<SubflowCallKey, Guid> _subflowCalls = new();
        private readonly IStepFactory _stepFactory;
        private readonly FlowGraph _graph;

        public FlowInstance(IFlowDefinition definition,IStepFactory stepFactory)
        {
            _stepFactory = stepFactory;
            _graph = definition.Flow;
            CurrentStepId = _graph.StartNodeId;
        }

        public IFlowStep GetStep(Guid stepId)
        {
            var node = _graph.GetStep(stepId);
            return _stepFactory.Resolve(node.StepType);
        }

        public bool TryResolveNext(Guid currentStepId, IFlowContext ctx,out NextStepResult next)
        {
            next = null;
            
            var edge = _graph.GetNextEdge(currentStepId, ctx);
            if (edge == null) return false;

            var transformer = _graph.GetTransformer(edge.Id);
            var input = transformer != null
                ? transformer.Transform(ctx, Payload)
                : Payload;

            next = new NextStepResult(edge.ToNodeId, input);
            return true;
        }

        public void RegisterSubflowCall(SubflowCallKey key, Guid childInstanceId) =>
            _subflowCalls[key] = childInstanceId;

        public bool TryGetSubflowCall(SubflowCallKey key, out Guid childInstanceId) =>
            _subflowCalls.TryGetValue(key, out childInstanceId);
    }
}
