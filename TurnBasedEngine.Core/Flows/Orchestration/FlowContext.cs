using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Commands;
using TurnBasedEngine.Core.EventBus;
using TurnBasedEngine.Core.Flows.Execution;
using TurnBasedEngine.Core.Flows.Factories;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Orchestration
{
    public class FlowContext
    {
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly ICommandResolver _commandResolver;
        public FlowContext(IFlowOrchestrator orchestrator,ICommandResolver commandResolver)
        {
            _flowOrchestrator = orchestrator;
        }

        public Task<StepValue> Command(ICommand command)
        {
            var flowRequest = _commandResolver.Resolve(command);
            return _flowOrchestrator.ExecuteFlowStartRequest(flowRequest);
        }
    }
}
