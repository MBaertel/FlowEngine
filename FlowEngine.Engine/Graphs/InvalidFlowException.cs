using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Flows.Graphs
{
    public class InvalidFlowException : InvalidOperationException
    {
        public string[] Errors { get; }
        public InvalidFlowException(string[] errors) 
            :base("Flow is malformed") 
        {
            Errors = errors;
        }
    }
}
