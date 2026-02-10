using FlowEngine.Engine.Flows.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Steps
{
    public interface IStepFactory
    {
        IFlowStep Resolve(Type stepType);
        IFlowStep Resolve<TStep>() where TStep : IFlowStep;
    }
}
