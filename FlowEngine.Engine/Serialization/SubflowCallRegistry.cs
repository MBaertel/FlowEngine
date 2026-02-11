using FlowEngine.Engine.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Serialization
{
    public class SubflowCallRegistry : ISubflowCallRegisty
    {
        private readonly Dictionary<SubflowCallKey, FlowWait> _waits = new();

        public void Register(SubflowCallKey key, FlowWait wait) =>
            _waits.TryAdd(key, wait);

        public bool TryGet(SubflowCallKey key, out FlowWait wait) =>
            _waits.TryGetValue(key, out wait);
    }
}
