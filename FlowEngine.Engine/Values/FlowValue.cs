using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Flows.Values
{
    /// <summary>
    /// Base class for all step outputs
    /// </summary>
    public abstract record FlowValue
    {

        public static readonly EmptyValue Empty = new EmptyValue();

        public static FlowValue Wrap<T>(T value)
        {
            if (value is FlowValue fv)
                return fv;

            if (value is EmptyValue emptyValue)
                return EmptyValue.Empty;

            return new TypedValue<T>(value);
        }
    }

    public sealed record EmptyValue : FlowValue { }

    public record TypedValue<T>(T Value) : FlowValue;
}
