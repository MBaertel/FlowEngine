using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Systems
{
    public interface ISystem
    {
        Type InputType { get; }

        Type OutputType { get; }
        public Task<object> RunAsync(object input);
    }

    public interface ISystem<TIn,TOut> : ISystem
    {
        public Task<TOut> RunAsync(TIn input);
    }
}
