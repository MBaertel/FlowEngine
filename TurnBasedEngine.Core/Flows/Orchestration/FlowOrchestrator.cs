using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using TurnBasedEngine.Core.EventBus;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Execution;
using TurnBasedEngine.Core.Flows.Factories;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Orchestration
{
    public class FlowOrchestrator : IFlowOrchestrator
    {
        private readonly IFlowFactory _flowFactory;
        private readonly List<IFlowRunner> _activeRunners = new();

        public IReadOnlyCollection<IFlowRunner> ActiveRunners => _activeRunners.ToList().AsReadOnly();

        public FlowOrchestrator(IFlowFactory flowFactory)
        {
            _flowFactory = flowFactory;
        }

        public async Task<StepValue> ExecuteFlowAsync(IFlowDefinition flowDefinition, StepValue? input = null)
        {
            if(flowDefinition == null) throw new ArgumentNullException(nameof(flowDefinition));
            input = input ?? StepValue.Empty;

            var flow = _flowFactory.CreateInstance(flowDefinition,input);
            var runner = new FlowRunner(flow);
            _activeRunners.Add(runner);

            try
            {
                return await runner.WaitForCompletion();
            }
            finally
            {
                _activeRunners.Remove(runner);
            }
        }

        public async Task StepAllAsync()
        {
            foreach (var runner in _activeRunners)
            {
                if(!runner.IsWaiting)
                    await runner.StepAsync();
            }
        }
    }
}
