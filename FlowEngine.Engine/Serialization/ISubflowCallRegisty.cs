using FlowEngine.Engine.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Serialization
{
    public interface ISubflowCallRegisty
    {
        bool TryGet(SubflowCallKey key,out FlowWait wait);
        void Register(SubflowCallKey key, FlowWait wait);
    }
}
