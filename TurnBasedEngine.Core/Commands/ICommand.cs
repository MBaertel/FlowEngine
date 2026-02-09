using System;
using System.Collections.Generic;
using System.Text;
using TurnBasedEngine.Core.Flows.Values;

namespace TurnBasedEngine.Core.Commands
{
    public interface ICommand
    {
        string FlowName { get; }
        object? CommandValue { get; }
    }

    public interface ICommand<TCommand> : ICommand
    {
        new TCommand CommandValue { get; }
    }
}
