using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Steps;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Tests.EngineTests.TestSteps
{
    internal class TestSubflowStep : FlowStepBase<MathInput, float>
    {
        public override async Task<float> ExecuteAsync(IStepContext ctx, MathInput input)
        {
            var flow = ctx.GetFlowByName<MathInput,float>("MathTest");
            var result = await ctx.ExecuteSubFlow(flow, input);

            return result;
        }

        public override void Undo(MathInput input, float output)
        {
            throw new NotImplementedException();
        }
    }
}
