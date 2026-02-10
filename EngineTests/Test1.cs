using Microsoft.Extensions.DependencyInjection;
using FlowEngine.Engine;
using FlowEngine.Core.Commands;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Orchestration;

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
            var flowRegistry = engine.Services.GetRequiredService<IFlowDefinitionRegistry>();
            
            Assert.IsNotNull(orchestrator);
            Assert.IsNotNull(flowRegistry);
        }
    }
}
