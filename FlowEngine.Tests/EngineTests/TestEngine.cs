using FlowEngine.Engine;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Tests.EngineTests.TestFlows;
using FlowEngine.Tests.EngineTests.TestSteps;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Tests.EngineTests
{
    internal class TestEngine : EngineBase
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFlowStep, MathStep>();
            services.AddSingleton<IFlowDefinition, MathTestFlow>();
            services.AddSingleton<IFlowDefinition, MathTestFlowMult>();
        }

        public Task<FlowValue> RunFlow(IFlowDefinition definition,FlowValue input)
        {
            var orchestrator = this.Services.GetRequiredService<IFlowOrchestrator>();
            return orchestrator.ExecuteFlowAsync(definition, input);
        }  
    }
}
