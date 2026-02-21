using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using FlowEngine.Core;
using FlowEngine.Engine;
using FlowEngine.Integration.Systems;
using FlowEngine.Integration.Services;
using Microsoft.Extensions.DependencyInjection;
using FlowEngine.Integration.Converters;

namespace FlowEngine.Integration.Godot.Nodes
{
    [GlobalClass]
    public class FlowEngine : Node
    {
        private EngineBase _flowEngine;
        private ISystemRegistry _systemRegistry;
        private IServiceRegistry _serviceRegistry;
        private IConverterRegistry _converterRegistry;
        private IDecisionManager _decisionManager;

        public IServiceRegistry StateServices => _serviceRegistry;

        public FlowEngine() 
            : base()
        {
            _flowEngine = new StandardEngine();
        }

        public override void _Ready()
        {
            base._Ready();
            _systemRegistry = _flowEngine.Services.GetRequiredService<ISystemRegistry>();
            _converterRegistry = _flowEngine.Services.GetRequiredService<IConverterRegistry>();
            _decisionManager = _flowEngine.Services.GetRequiredService<IDecisionManager>();
            FindSystems();
        }

        private void FindSystems()
        {
            var systems = new List<ISystem>();
            void Traverse(Node node)
            {
                if (node is ISystem system)
                {
                    systems.Add(system);
                }

                foreach (var child in node.GetChildren())
                    Traverse(child as Node);
            }
            Traverse(this);
            foreach (var system in systems)
            {
                _systemRegistry.RegisterSystem(system);
            }
        }

        public void RegisterSystem(ISystem system) =>
            _systemRegistry.RegisterSystem(system);

        public void RegisterService(IGameStateService service) =>
            _serviceRegistry.RegisterService(service);

        public void RegisterConverter(IEventConverter converter) =>
            _converterRegistry.RegisterConverter(converter);

    }
}
