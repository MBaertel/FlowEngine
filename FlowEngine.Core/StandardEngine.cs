using FlowEngine.Core.Commands;
using FlowEngine.Core.Events.Bus;
using FlowEngine.Engine;
using FlowEngine.Engine.Flows.Orchestration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core
{
    public class StandardEngine : EngineBase
    {
        private readonly ICommandRouter _commandRouter;

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>();
            services.AddSingleton<ICommandRouter, CommandRouter>();
        }

        public async Task<TResult> RunCommand<TInput, TResult>(ICommand<TInput, TResult> command) =>
            await _commandRouter.RunCommand(command);

        public void RegisterCommandBinding<TCommand, TCommandIn, TFlowIn>(Func<TCommandIn, TFlowIn> adapter) 
            where TCommand : ICommand
            where TFlowIn : notnull 
            => _commandRouter.Register<TCommand, TCommandIn, TFlowIn>(adapter);
    }
}
