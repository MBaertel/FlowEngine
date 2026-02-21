using FlowEngine.Integration.Decisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Services
{
    public interface IDecisionManager : IStateService
    {
        IDecisionProvider GetCurrent();
        void SetCurrent(IDecisionProvider decisionProvider);
    }
}
