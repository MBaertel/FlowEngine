using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Steps;
using FlowEngine.Engine.Serialization;

namespace FlowEngine.Engine
{
    public abstract class EngineBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly IFlowDefinitionRegistry _flowRegistry;

        public IServiceProvider Services => _serviceProvider;

        public EngineBase()
        {
            var services = new ServiceCollection();
            RegisterBaseServices(services);
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            _flowOrchestrator = Services.GetRequiredService<IFlowOrchestrator>();
            _flowRegistry = Services.GetRequiredService<IFlowDefinitionRegistry>();
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        private void RegisterBaseServices(IServiceCollection services)
        {
            services.AddSingleton<IFlowDefinitionRegistry, FlowDefinitionRegistry>();
            services.AddSingleton<IFlowOrchestrator, FlowOrchestrator>();
            services.AddSingleton<ISubflowCallRegisty, SubflowCallRegistry>();
            services.AddSingleton<IStepFactory, StepFactory>();
        }

        public Task TickAsync() =>
            _flowOrchestrator.StepAllAsync();

        public void RegisterFlow(IFlowDefinition definition) =>
            _flowRegistry.Register(definition);
    }
}
