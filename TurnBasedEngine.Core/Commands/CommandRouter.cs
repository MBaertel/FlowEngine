using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Commands
{
    public class CommandRouter : ICommandRouter
    {
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly IFlowDefinitionRegistry _flowRegistry;

        public CommandRouter(IFlowDefinitionRegistry registry,IFlowOrchestrator orchestrator)
        {
            _flowOrchestrator = orchestrator;
            _flowRegistry = registry;
        }

        public async Task<TResult> RunCommand<TResult>(ICommand command)
        {
            var flow = _flowRegistry.GetByName(command.FlowName);
            
            NodeValue inputValue = NodeValue.Wrap(command.CommandValue);
            var result = await _flowOrchestrator.ExecuteFlowAsync(flow, inputValue);

            return result.Unwrap<TResult>();
        }
    }
}
