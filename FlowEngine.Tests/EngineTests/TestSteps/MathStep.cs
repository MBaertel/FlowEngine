using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FlowEngine.Tests.EngineTests.TestSteps
{
    internal class MathStep : IFlowStep<MathInput, float>
    {
        public Guid Id => Guid.NewGuid();

        public Task<FlowValue> ExecuteAsync(IFlowContext ctx, FlowValue input)
        {
            var mathInput = input.Unwrap<MathInput>();

            float output;
            switch (mathInput.mode)
            {
                case MathModes.Add:
                    output = mathInput.x + mathInput.y;
                    break;
                case MathModes.Subtract:
                    output = mathInput.x + mathInput.y;
                    break;
                case MathModes.Multiply:
                    output = mathInput.x * mathInput.y;
                    break;
                case MathModes.Divide:
                    output = mathInput.x / mathInput.y;
                    break;
                default:
                    output = 0;
                    break;
            }

            return Task.FromResult(FlowValue.Wrap(output));
        }

        public void Undo(FlowValue input, FlowValue output)
        {
            throw new NotImplementedException();
        }
    }

    internal record MathInput(float x, float y, MathModes mode);

    internal enum MathModes
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}
