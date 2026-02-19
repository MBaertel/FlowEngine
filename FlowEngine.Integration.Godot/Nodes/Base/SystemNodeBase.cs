using FlowEngine.Integration.Godot.Services;
using FlowEngine.Integration.Systems;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Integration.Godot.Nodes.Base
{
    public abstract class SystemNodeBase<TIn,TOut> : Node, ISystem<TIn,TOut>
    {
        public Type InputType => typeof(TIn);
        public Type OutputType => typeof(TOut);
        string ISystem.Name => Name;

        private ServiceRegistry _serviceRegistry;

        public abstract Task<TOut> RunAsync(TIn input);

        protected virtual void OnReady(ServiceRegistry services) { }

        public async Task<object> RunAsync(object input)
        {
            if (input is TIn typedInput)
                await RunAsync(typedInput);
            throw new InvalidCastException($"Input type was {input.GetType()}, expected {typeof(TIn)}");
        }

        public override void _Ready()
        {
            var registry = GetNode<ServiceRegistry>("../ServiceRegistry");
            var flowEngine = GetNode<FlowEngineNode>("../FlowEngineNode");
            _serviceRegistry = registry;
            OnReady(_serviceRegistry);
        }
    }
}
