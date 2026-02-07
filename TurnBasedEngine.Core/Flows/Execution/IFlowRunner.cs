using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Instances;
using TurnBasedEngine.Core.Flows.Orchestration;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Execution
{
    public interface IFlowRunner
    {
        public bool IsWaiting { get; }
        Task<StepValue> WaitForCompletion();
        Task StepAsync();
    }
}
