using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Commands
{
    public interface ICommandRouter
    {
        public Task<TResult> RunCommand<TInput, TResult>(ICommand<TInput, TResult> command);

        public void Register<TCommand, TCommandIn, TFlowIn>(Func<TCommandIn, TFlowIn>? adapter = null)
            where TCommand : ICommand;
    }
}
