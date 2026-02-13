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
            services.AddSingleton<IFlowStep, TestSubflowStep>();
            services.AddSingleton<IFlowDefinition, MathTestFlow>();
            services.AddSingleton<IFlowDefinition, MathTestFlowMany>();
            services.AddSingleton<IFlowDefinition, MathSubflowTestFlow>();
            services.AddSingleton<IFlowDefinition, MathSubflowTestMany>();
            services.AddSingleton<IFlowDefinition, MathSubflowTwiceTestFlow>();
        }

        public Task<object> RunFlow(IFlowDefinition definition,object input)
        {
            var orchestrator = this.Services.GetRequiredService<IFlowOrchestrator>();
            return orchestrator.ExecuteFlowAsync(definition, input);
        } 
        
        public Task<TResult> RunTypedFlow<TInput,TResult>(IFlowDefinition<TInput,TResult> definition,TInput input)
        {
            var orchestrator = this.Services.GetRequiredService<IFlowOrchestrator>();
            return orchestrator.ExecuteFlowAsync(definition, input);
        }
    }
}
