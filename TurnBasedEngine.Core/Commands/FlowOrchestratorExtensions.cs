using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Commands
{
    public static class FlowOrchestratorExtensions
    {
        public static Task<StepValue> ExecuteFlowStartRequest(this IFlowOrchestrator flowOrchestrator,FlowStartRequest request)
        {
            return flowOrchestrator.ExecuteFlowAsync(request.Definition, request.Input);
        }
    }
}
