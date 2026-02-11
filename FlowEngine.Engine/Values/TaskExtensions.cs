using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Values
{
    public static class TaskExtensions
    {
        public static async Task<object> AwaitAndBox<TResult>(Task<TResult> task)
        {
            return await task.ConfigureAwait(false)!;
        }
    }
}
