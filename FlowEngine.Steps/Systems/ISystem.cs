using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Systems
{
    public interface ISystem
    {
        string Name { get; }
        Task<object> RunAsync(object input);
    }

    public interface ISystem<TIn,TOut> : ISystem
    {
        Task<TOut> RunAsync(TIn input);
    }
}
