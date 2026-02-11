using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Core.Commands
{
    public static class CommandLambda
    {
        public static Func<TInput, object> CommandTransformer<TInput, TOutput>(Func<TInput, TOutput> lambda) =>
            input => lambda(input);
    }
}
