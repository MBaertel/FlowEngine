using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Execution.Context
{
    public class StepContext : IStepContext
    {
        private readonly IFlowInstance _instance;
        private readonly IFlowOrchestrator _orchestrator;
        private readonly IFlowDefinitionRegistry _flowRegisty;

        private readonly Guid _stepId;
        private int _callId = 0;

        public StepContext(IFlowInstance instance,IFlowOrchestrator orchestrator,IFlowDefinitionRegistry registry,Guid stepId) 
        {
            _orchestrator = orchestrator;
            _flowRegisty = registry;
            _instance = instance;
            _stepId = stepId;
        }

        public FlowWait ExecuteSubflow(IFlowDefinition flowDefinition, object input)
        {
            var runner = _orchestrator.AddFlow(flowDefinition, input);

            var subflowKey = new SubflowCallKey(_stepId, _callId);
            _instance.RegisterSubflowCall(subflowKey, runner.Instance.InstanceId);
            _callId++;

            var wait = new FlowWait(runner.Instance.InstanceId);
            wait.BindRunner(runner);

            return wait;
        }

        public FlowWait<TOut> ExecuteSubFlow<TIn, TOut>(IFlowDefinition<TIn, TOut> flowDefinition, TIn input)
        {
            var runner = _orchestrator.AddFlow<TIn,TOut>(flowDefinition, input);

            var subflowKey = new SubflowCallKey(_stepId, _callId);
            _instance.RegisterSubflowCall(subflowKey, runner.Instance.InstanceId);
            _callId++;

            var wait = new FlowWait<TOut>(runner.Instance.InstanceId);
            wait.BindRunner(runner);

            return wait;
        }

        public IFlowDefinition GetFlowById(Guid id) =>
            _flowRegisty.GetById(id);

        public IFlowDefinition<TIn, TOut> GetFlowById<TIn, TOut>(Guid id) =>
            _flowRegisty.GetById<TIn, TOut>(id);

        public IFlowDefinition GetFlowByName(string name) =>
            _flowRegisty.GetByName(name);

        public IFlowDefinition<TIn, TOut> GetFlowByName<TIn, TOut>(string name) =>
            _flowRegisty.GetByName<TIn, TOut>(name);
    }
}
