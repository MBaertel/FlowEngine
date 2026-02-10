using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Core.Commands
{
    public interface ICommand
    {
        string FlowName { get; }
        object? InputValue { get; }
        Type ValueType { get; }
    }

    public interface ICommand<TInput,TOutput> : ICommand
    {
        new TInput InputValue { get; }
    }
}
