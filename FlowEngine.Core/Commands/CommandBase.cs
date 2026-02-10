using FlowEngine.Engine.Flows.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Commands
{
    public abstract record Command<TInput,TOutput>(string FlowName,TInput InputValue)
        : ICommand<TInput,TOutput>
    {
        object? ICommand.InputValue => InputValue;
        public TInput InputValue => InputValue;
        public Type ValueType => typeof(TInput);
    }

    public abstract record OutputOnlyCommand<TOutput>(string FlowName)
        : Command<EmptyValue,TOutput>(FlowName,FlowValue.Empty);

    public abstract record InputOnlyCommand<TInput>(string FlowName,TInput InputValue)
        : Command<TInput,EmptyValue>(FlowName,InputValue);
}
