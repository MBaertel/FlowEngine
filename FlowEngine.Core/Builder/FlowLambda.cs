using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Engine.Flows.Graphs.Builder
{
    public static class FlowLambda
    {
        public static Func<IFlowContext,FlowValue,FlowValue> Transform<TIn,TOut>(Func<IFlowContext,TIn,TOut> lambda)
        {
            return (ctx, value) =>
            {
                if (value is not TypedValue<TIn> typed)
                    throw new InvalidOperationException($"Expected {typeof(TIn)} but got {value.GetType()}");

                var result = lambda(ctx, typed.Value);
                return new TypedValue<TOut>(result);
            };
        }

        public static Func<IFlowContext,FlowValue,Task<FlowValue>> TransformAsync<TIn,TOut>(Func<IFlowContext,TIn,Task<TOut>> lambda)
        {
            return async (ctx, value) =>
            {
                if (value is not TypedValue<TIn> typed)
                    throw new InvalidOperationException($"Expected {typeof(TIn)} but got {value.GetType()}");

                var result = await lambda(ctx, typed.Value);
                return new TypedValue<TOut>(result);
            };
        }

        public static Func<IFlowContext,FlowValue,bool> When<TIn>(Func<IFlowContext,TIn,bool> predicate)
        {
            return (ctx, value) =>
            {
                if (value is not TypedValue<TIn> typed)
                    throw new InvalidOperationException($"Expected {typeof(TIn)} but got {value.GetType()}");

                return predicate(ctx, typed.Value);
            };
        }
    }
}
