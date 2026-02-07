using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Steps;

namespace TurnBasedEngine.Core.Flows.Factories
{
    public interface IStepFactory
    {
        IFlowStep CreateStep(Type stepType);
        TStep CreateStep<TStep>() where TStep : IFlowStep;
    }
}
