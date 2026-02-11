using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Steps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FlowEngine.Tests.EngineTests.TestSteps
{
    internal class MathStep : FlowStepBase<MathInput,float>
    {
        public Guid Id => Guid.NewGuid();

        public override Task<float> ExecuteAsync(IFlowContext ctx, MathInput input)
        {
            float output;
            switch (input.mode)
            {
                case MathModes.Add:
                    output = input.x + input.y;
                    break;
                case MathModes.Subtract:
                    output = input.x + input.y;
                    break;
                case MathModes.Multiply:
                    output = input.x * input.y;
                    break;
                case MathModes.Divide:
                    output = input.x / input.y;
                    break;
                default:
                    output = 0;
                    break;
            }

            return Task.FromResult(output);
        }

        public override void Undo(MathInput input, float output)
        {
            throw new NotImplementedException();
        }
    }

    public record MathInput(float x, float y, MathModes mode);

    public enum MathModes
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}
