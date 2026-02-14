using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Systems
{
    public abstract class SystemBase<TIn, TOut>
        : ISystem<TIn, TOut>
    {
        public Type InputType => typeof(TIn);
        public Type OutputType => typeof(TOut);

        public abstract Task<TOut> RunAsync(TIn input);

        public async Task<object> RunAsync(object input)
        {
            if(input is TIn typedInput)
            {
                var result = await RunAsync(typedInput);
                if(result is TOut typedOutput)
                    return result;
                throw new InvalidOperationException($"Result was of type {result.GetType}, expected {typeof(TOut)}");
            }
            throw new InvalidCastException($"Input was of type {input.GetType}, expected {typeof(TIn)}");
        }
    }
}
