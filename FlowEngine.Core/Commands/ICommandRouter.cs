using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Commands
{
    public interface ICommandRouter
    {
        Task<TResult> RunCommand<TResult>(ICommand command);
        Task<TResult> RunCommand<TInput,TResult>(ICommand<TInput,TResult> command);

        public void Register<TCommand, TCommandIn, TFlowIn>(
            Func<TCommandIn, TFlowIn> adapter)
            where TCommand : ICommand
            where TFlowIn : notnull;
    }
}
