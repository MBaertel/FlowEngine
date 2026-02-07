using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Flows.Steps
{
    /// <summary>
    /// Base class for all step outputs
    /// </summary>
    public abstract record StepValue
    {
        public static readonly StepValue Empty = new EmptyStepValue();
        private sealed record EmptyStepValue : StepValue { }
    }
}
