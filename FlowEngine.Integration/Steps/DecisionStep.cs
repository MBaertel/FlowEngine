using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Integration.Decisions;
using FlowEngine.Integration.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Steps
{
    public class DecisionStep<TChoice, TResult>
        : IFlowStep<TChoice,TResult>
        where TChoice : IChoice
        where TResult : IChoiceResult
    {
        private readonly IDecisionProvider _decisionProvider;

        public DecisionStep(IDecisionManager decisionManager)
        {
            _decisionProvider = decisionManager.GetCurrent();
        }

        public async Task<TResult> ExecuteAsync(IStepContext ctx, TChoice input)
        {
            var result = await _decisionProvider.RequestDecisionAsync<TChoice, TResult>(input);
            return result;
        }

        public async Task<object> ExecuteAsyncUntyped(IStepContext ctx, object input)
        {
            if (input is TChoice typed)
                return await ExecuteAsync(ctx,typed);

            throw new InvalidCastException($"Input was {input.GetType()}, expected {typeof(TChoice)}");
        }

        public void Undo(TChoice input, TResult output)
        {
            throw new NotImplementedException();
        }

        public void UndoUntyped(object input, object output)
        {
            throw new NotImplementedException();
        }
    }
}
