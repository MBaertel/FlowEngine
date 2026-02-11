using FlowEngine.Engine.Flows.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Commands
{
    public class CommandBinding<TIn>
    {
        public Type CommandType { get; }

        public Type CommandInputType => typeof(TIn);

        public Func<TIn,object>? InputAdapter { get; }

        public CommandBinding(Type commandType, Func<TIn, object>? inputAdapter = null)
        {
            CommandType = commandType;
            InputAdapter = inputAdapter;
        }
    }


}
