using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Execution;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Orchestration
{
    public interface IFlowOrchestrator
    {
        Task<StepValue> ExecuteFlowAsync(IFlowDefinition flow, StepValue? input = null);

        Task StepAllAsync();

        IReadOnlyCollection<IFlowRunner> ActiveRunners { get; }
    }
}
