using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs.Builder
{
    public static class FlowLambda
    {
        public static Func<IFlowContext,object,object> Transform<TIn,TOut>(Func<IFlowContext,TIn,TOut> lambda)
        {
            return (ctx, value) =>
            {
                if (value is not TIn typed)
                    throw new InvalidOperationException($"Expected {typeof(TIn)} but got {value.GetType()}");

                var result = lambda(ctx, typed);
                return result;
            };
        }

        public static Func<IFlowContext,object,Task<object>> TransformAsync<TIn,TOut>(Func<IFlowContext,TIn,Task<TOut>> lambda)
        {
            return async (ctx, value) =>
            {
                if (value is not TIn typed)
                    throw new InvalidOperationException($"Expected {typeof(TIn)} but got {value.GetType()}");

                var result = await lambda(ctx, typed);
                return result;
            };
        }

        public static Func<IFlowContext,object,bool> When<TIn>(Func<IFlowContext,TIn,bool> predicate)
        {
            return (ctx, value) =>
            {
                if (value is not TIn typed)
                    throw new InvalidOperationException($"Expected {typeof(TIn)} but got {value.GetType()}");

                return predicate(ctx, typed);
            };
        }
    }
}
