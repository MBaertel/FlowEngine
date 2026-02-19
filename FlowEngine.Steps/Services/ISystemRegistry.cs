using FlowEngine.Integration.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Services
{
    public interface ISystemRegistry
    {
        void RegisterSystem(ISystem system);
        bool TryGetSystem<TIn, TOut>(out ISystem<TIn,TOut> system);
        bool TryGetSystem(string name, out ISystem system);
    }
}
