using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Flows.Values
{
    public static class ValueExtensions
    {
        public static TResult Unwrap<TResult>(this FlowValue value)
        {
            // Expected empty result
            if (value is EmptyValue)
            {
                if (typeof(TResult) == typeof(void) ||
                    typeof(TResult) == typeof(EmptyValue))
                    return default!;

                throw new InvalidOperationException(
                    $"Flow returned EmptyValue but command expects {typeof(TResult).Name}");
            }

            if (value is TypedValue<TResult> typed)
                return typed.Value;

            throw new InvalidOperationException(
                $"Flow returned {value.GetType().Name} but command expects {typeof(TResult).Name}");
        }
    }
}
