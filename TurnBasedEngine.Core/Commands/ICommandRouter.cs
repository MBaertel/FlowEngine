using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Commands
{
    public interface ICommandRouter
    {
        Task<TResult> RunCommand<TResult>(ICommand command);
    }
}
