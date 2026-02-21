using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Decisions
{
    public interface IDecisionProvider
    {
        Task<TResult> RequestDecisionAsync<TChoice,TResult>(TChoice choice)
            where TChoice : IChoice
            where TResult : IChoiceResult;
    }
}
