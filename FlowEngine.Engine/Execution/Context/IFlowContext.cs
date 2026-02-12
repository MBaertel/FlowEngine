using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Execution.Context
{
    public interface IFlowContext
    {
        object Payload { get; }

        void SetVar<T>(string name, T value);
        bool TryGetVar<T>(string name, out T value);
    }
}
