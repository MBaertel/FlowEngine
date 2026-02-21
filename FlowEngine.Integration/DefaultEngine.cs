using FlowEngine.Core;
using FlowEngine.Integration.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Integration
{
    public class DefaultEngine : CoreEngine
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISystemRegistry, SystemRegistry>();
            services.AddSingleton<IServiceRegistry, ServiceRegistry>();
            services.AddSingleton<IConverterRegistry, ConverterRegistry>();

            base.ConfigureServices(services);
        }
    }
}
