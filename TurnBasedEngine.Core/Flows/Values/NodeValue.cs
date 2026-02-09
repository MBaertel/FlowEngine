using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Flows.Values
{
    /// <summary>
    /// Base class for all step outputs
    /// </summary>
    public abstract record NodeValue
    {

        public static readonly EmptyValue Empty = new EmptyValue();

        public static NodeValue Wrap(object? value)
        {
            if (value == null)
                return NodeValue.Empty;

            if (value is NodeValue nv)
                return nv;

            var type = typeof(TypedValue<>)
                .MakeGenericType(value.GetType());

            return (NodeValue)Activator.CreateInstance(type, value)!;
        }
    }

    public sealed record EmptyValue : NodeValue { }

    public record TypedValue<T>(T Value) : NodeValue;
}
