using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Execution;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;
using FlowEngine.Engine.Values;

namespace FlowEngine.Engine.Execution.Context
{
    public class FlowContext : IFlowContext
    {

        public object Payload
        {
            get => _instance.Payload;
            set => _instance.Payload = value;
        }

        private readonly IFlowInstance _instance;

        public FlowContext(IFlowInstance instance)
        {
            _instance = instance;
        }

        public void SetVar<T>(string name, T value) =>
            _instance.Variables[name] = value;

        public bool TryGetVar<T>(string name, out T value)
        {
            if( _instance.Variables.TryGetValue(name,out var variableValue) &&
                variableValue is T typedValue)
            {
                value = typedValue;
                return true;
            }
            value = default;
            return false;
        }
    }
}
