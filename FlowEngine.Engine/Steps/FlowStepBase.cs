using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Steps
{
    public abstract class FlowStepBase<TIn, TOut> : IFlowStep<TIn, TOut>
    {

        public async Task<object> ExecuteAsyncUntyped(IFlowContext ctx, object input)
        {
            if (input is not TIn typed)
                throw new InvalidCastException($"Input was {input.GetType()} ,expected {typeof(TIn)}");

            return await ExecuteAsync(ctx, typed);
        }

        public void UndoUntyped(object input, object output)
        {
            if(input is not TIn typedIn)
                throw new InvalidCastException($"Input was {input.GetType()} ,expected {typeof(TIn)}");
            if(output is not TOut typedOut)
                throw new InvalidCastException($"Input was {output.GetType()} ,expected {typeof(TOut)}");

            Undo(typedIn, typedOut);
        }

        public abstract Task<TOut> ExecuteAsync(IFlowContext ctx, TIn input);
        public abstract void Undo(TIn input, TOut output);
    }
}
