using FlowEngine.Integration.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Services
{
    public class SystemRegistry : ISystemRegistry
    {
        private readonly Dictionary<(Type, Type), ISystem> _systemsByType = new();
        private readonly Dictionary<string, ISystem> _systemsByName = new();
        public void RegisterSystem(ISystem system)
        {
            _systemsByType[(system.InputType, system.OutputType)] = system;
            _systemsByName[system.Name] = system;
        }

        public bool TryGetSystem<TIn, TOut>(out ISystem<TIn, TOut> system)
        {
            if(_systemsByType.TryGetValue((typeof(TIn),typeof(TOut)),out var untypedSystem))
            {
                if (untypedSystem is ISystem<TIn, TOut> typedSystem)
                {
                    system = typedSystem;
                    return true;
                }
            }
            system = null;
            return false;
        }
        public bool TryGetSystem(string name, out ISystem system)
        {
            if (_systemsByName.TryGetValue(name, out system))
                return true;
            return false;
        }
    }
}
