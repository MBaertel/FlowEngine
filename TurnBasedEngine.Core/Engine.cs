using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Commands;
using TurnBasedEngine.Core.Events.Bus;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Orchestration;

namespace TurnBasedEngine.Core
{
    public class Engine
    {
        private readonly IServiceProvider _serviceProvider;

        public IFlowOrchestrator Orchestrator { get; }
        public IEventBus EventBus { get; }
        public IServiceProvider Services => _serviceProvider;

        public Engine()
        {
            var services = new ServiceCollection();
            _serviceProvider = ConfigureServices(services);

            Orchestrator = _serviceProvider.GetRequiredService<IFlowOrchestrator>();
            EventBus = _serviceProvider.GetRequiredService<IEventBus>();
        }

        private IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFlowDefinitionRegistry, FlowDefinitionRegistry>();
            serviceCollection.AddSingleton<IFlowOrchestrator,FlowOrchestrator>();
            serviceCollection.AddSingleton<ICommandRouter,CommandRouter>();
            serviceCollection.AddSingleton<IEventBus, EventBus>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
