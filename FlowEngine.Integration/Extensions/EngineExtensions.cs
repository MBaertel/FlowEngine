using FlowEngine.Engine;
using FlowEngine.Integration.Converters;
using FlowEngine.Integration.Services;
using FlowEngine.Integration.Systems;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Extensions
{
    public static class EngineExtensions
    {
        public static void RegisterSystem(this EngineBase engine,ISystem system)
        {
            var registry = engine.Services.GetRequiredService<ISystemRegistry>();
            registry.RegisterSystem(system);
        }

        public static void RegisterConverter(this EngineBase engine,IEventConverter converter)
        {
            var registry = engine.Services.GetRequiredService<IConverterRegistry>();
            registry.RegisterConverter(converter);
        }
    }
}
