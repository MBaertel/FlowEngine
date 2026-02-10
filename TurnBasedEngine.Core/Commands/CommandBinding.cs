using FlowEngine.Engine.Flows.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Commands
{
    public class CommandBinding
    {
        public Type CommandType { get; }
        public Func<object?, FlowValue> InputAdapter { get; }

        public static CommandBinding Default => new CommandBinding(typeof(ICommand),x => FlowValue.Wrap(x));

        public CommandBinding(Type commandType, Func<object?, FlowValue> inputAdapter)
        {
            CommandType = commandType;
            InputAdapter = inputAdapter;
        }
    }
}
