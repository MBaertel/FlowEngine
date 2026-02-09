using Microsoft.Extensions.DependencyInjection;
using TurnBasedEngine.Core;
using TurnBasedEngine.Core.Commands;
using TurnBasedEngine.Core.Flows.Definitions;
using TurnBasedEngine.Core.Flows.Orchestration;

namespace EngineTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void CreateEngineTest()
        {
            var engine = new Engine();

            var orchestrator = engine.Orchestrator;
            var eventBus = engine.EventBus;
            var commandRouter = engine.Services.GetRequiredService<ICommandRouter>();
            var flowRegistry = engine.Services.GetRequiredService<IFlowDefinitionRegistry>();
            
            Assert.IsNotNull(orchestrator);
            Assert.IsNotNull(eventBus);
            Assert.IsNotNull(commandRouter);
            Assert.IsNotNull(flowRegistry);
        }
    }
}
