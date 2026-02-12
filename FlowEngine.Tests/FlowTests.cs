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

        private const int speedTestRuns = 10000;

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

            var flowTask = _engine.RunFlow(flowDefinition,input);

            while(!flowTask.IsCompleted)
            {
                await _engine.TickAsync();
            }

            var result = await flowTask;

            var value = (float)result;

            Assert.That(value, Is.EqualTo(2f));
        }

        [Test]
        public async Task MathFlowManyTest()
        {
            var flowDefinition = _flowRegistry.GetByName("MathTestMany");
            var input = new MathInput(1, 1, MathModes.Add);

            var flowTask = _engine.RunFlow(flowDefinition, input);

            while (!flowTask.IsCompleted)
            {
                await _engine.TickAsync();
            }

            var result = await flowTask;

            var value = (float)result;

            Assert.That(value, Is.EqualTo(501f));
        }

        [Test]
        public async Task MathFlowSpeedTestUntyped()
        {
            var flowDefinition = _flowRegistry.GetByName("MathTest");
            var input = new MathInput(1, 1, MathModes.Add);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<object>[speedTestRuns];
            for (int i = 0; i < speedTestRuns; i++)
            {
                tasks[i] = _engine.RunFlow(flowDefinition, input);
            }

            while(tasks.Any(x => !x.IsCompleted))
            {
                await _engine.TickAsync();
            }

            sw.Stop();
            Console.WriteLine($"Ran {speedTestRuns} flows in {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public async Task MathFlowSpeedTestTyped()
        {
            var flowDefinition = _flowRegistry.GetByName<MathInput, float>("MathTest");
            var input = new MathInput(1, 1, MathModes.Add);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<float>[speedTestRuns];
            for (int i = 0; i < speedTestRuns; i++)
            {
                tasks[i] = _engine.RunTypedFlow(flowDefinition, input);
            }

            while (tasks.Any(x => !x.IsCompleted))
            {
                await _engine.TickAsync();
            }

            sw.Stop();
            Console.WriteLine($"Ran {speedTestRuns} flows in {sw.ElapsedMilliseconds} ms");

        }

        [Test]
        public async Task MathFlowManySpeedTest()
        {
            var flowDefinition = _flowRegistry.GetByName("MathTestMany");
            var input = new MathInput(1, 1, MathModes.Add);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<object>[speedTestRuns];
            for (int i = 0; i < speedTestRuns; i++)
            {
                tasks[i] = _engine.RunFlow(flowDefinition, input);
            }

            while (tasks.Any(x => !x.IsCompleted))
            {
                await _engine.TickAsync();
            }

            sw.Stop();
            Console.WriteLine($"Ran {speedTestRuns} flows in {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public async Task MathFlowMultSpeedTestTyped()
        {
            var flowDefinition = _flowRegistry.GetByName<MathInput, float>("MathTestMany");
            var input = new MathInput(1, 1, MathModes.Add);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<float>[speedTestRuns];
            for (int i = 0; i < speedTestRuns; i++)
            {
                tasks[i] = _engine.RunTypedFlow(flowDefinition, input);
            }

            while (tasks.Any(x => !x.IsCompleted))
            {
                await _engine.TickAsync();
            }

            sw.Stop();
            Console.WriteLine($"Ran {speedTestRuns} flows in {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public async Task MathSubFlowTest()
        {
            var flowDefinition = _flowRegistry.GetByName<MathInput, float>("MathTestSubflow");
            var input = new MathInput(1, 1, MathModes.Add);

            var flowTask = _engine.RunFlow(flowDefinition, input);

            while (!flowTask.IsCompleted)
            {
                await _engine.TickAsync();
            }

            var result = await flowTask;

            var value = (float)result;

            Assert.That(value, Is.EqualTo(2f));
        }

        [Test]
        public async Task MathSubFlowSpeedTestTyped()
        {
            var flowDefinition = _flowRegistry.GetByName<MathInput, float>("MathTestSubflow");
            var input = new MathInput(1, 1, MathModes.Add);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<float>[speedTestRuns];
            for (int i = 0; i < speedTestRuns; i++)
            {
                tasks[i] = _engine.RunTypedFlow(flowDefinition, input);
            }

            while (tasks.Any(x => !x.IsCompleted))
            {
                await _engine.TickAsync();
            }

            sw.Stop();
            Console.WriteLine($"Ran {speedTestRuns} flows in {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public async Task MathSubFlowManySpeedTestTyped()
        {
            var flowDefinition = _flowRegistry.GetByName<MathInput, float>("MathSubflowTestMany");
            var input = new MathInput(1, 1, MathModes.Add);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var tasks = new Task<float>[speedTestRuns];
            for (int i = 0; i < speedTestRuns; i++)
            {
                tasks[i] = _engine.RunTypedFlow(flowDefinition, input);
            }

            while (tasks.Any(x => !x.IsCompleted))
            {
                await _engine.TickAsync();
            }

            sw.Stop();
            Console.WriteLine($"Ran {speedTestRuns} flows in {sw.ElapsedMilliseconds} ms");
        }
    }
}
