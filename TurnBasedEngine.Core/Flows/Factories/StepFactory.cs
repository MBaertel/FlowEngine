using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Instances;
using TurnBasedEngine.Core.Flows.Steps;
using Microsoft.Extensions.DependencyInjection;



namespace TurnBasedEngine.Core.Flows.Factories
{
    public class StepFactory : IStepFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public StepFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFlowStep CreateStep(Type stepType)
        {
            return (IFlowStep)_serviceProvider.GetRequiredService(stepType);
        }

        public TStep CreateStep<TStep>() where TStep : IFlowStep
        {
            return _serviceProvider.GetRequiredService<TStep>();
        }
    }
}
