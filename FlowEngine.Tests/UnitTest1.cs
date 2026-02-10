using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Tests.EngineTests;
using FlowEngine.Tests.EngineTests.TestSteps;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FlowEngine.Tests
{
    public class Tests
    {
        private TestEngine _engine;
        private IFlowDefinitionRegistry _flowRegistry;

        [SetUp]
        public void Setup()
        {
            _engine = new TestEngine();
            _flowRegistry = _engine.Services.GetRequiredService<IFlowDefinitionRegistry>();
        }

        [Test]
        public async Task MathFlowTest()
        {
            var flowDefinition = _flowRegistry.GetByName("MathTest");
            var input = new MathInput(1, 1, MathModes.Add);

            var flowTask = _engine.RunFlow(flowDefinition,FlowValue.Wrap(input));

            while(!flowTask.IsCompleted)
            {
                await _engine.TickAsync();
            }

            var result = await flowTask;

            var value = result.Unwrap<float>();

            Assert.That(value, Is.EqualTo(2f));
        }

        [Test]
        public async Task MathFlowMultTest()
        {
            var flowDefinition = _flowRegistry.GetByName("MathTestMult");
            var input = new MathInput(1, 1, MathModes.Add);

            var flowTask = _engine.RunFlow(flowDefinition, FlowValue.Wrap(input));

            while (!flowTask.IsCompleted)
            {
                await _engine.TickAsync();
            }

            var result = await flowTask;

            var value = result.Unwrap<float>();

            Assert.That(value, Is.EqualTo(4f));
        }

        [Test]
        public async Task MathFlowSpeedTest()
        {
            var flowDefinition = _flowRegistry.GetByName("MathTest");
            var input = new MathInput(1, 1, MathModes.Add);

            const int count = 100;

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<FlowValue>[count];
            for (int i = 0; i < count; i++)
            {
                tasks[i] = _engine.RunFlow(flowDefinition, FlowValue.Wrap(input));
            }

            bool run = true;
            while(run)
            {
                int ran = await _engine.TickAsync();
                run = ran > 0;

                await Task.Yield();
            }

            for (int i = 0; i < count; i++)
            {
                float value = tasks[i].Result.Unwrap<float>();
            }

            sw.Stop();
            Console.WriteLine($"Ran {count} flows in {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public async Task MathFlowMultSpeedTest()
        {
            var flowDefinition = _flowRegistry.GetByName("MathTestMult");
            var input = new MathInput(1, 1, MathModes.Add);

            const int count = 100;

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<FlowValue>[count];
            for (int i = 0; i < count; i++)
            {
                tasks[i] = _engine.RunFlow(flowDefinition, FlowValue.Wrap(input));
            }

            bool run = true;
            while (run)
            {
                int ran = await _engine.TickAsync();
                run = ran > 0;

                await Task.Yield();
            }

            for (int i = 0; i < count; i++)
            {
                float value = tasks[i].Result.Unwrap<float>();
            }

            sw.Stop();
            Console.WriteLine($"Ran {count} flows in {sw.ElapsedMilliseconds} ms");
        }
    }
}
