using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Values
{
    public class HistoryFrame
    {
        public Type StepType { get; init; }
        public Guid StepInstanceId { get; init; }
        public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.Now;

        public object? StepOutput { get; init; }
        public Dictionary<string,object?> RequiredUndoVariables { get; init; }
    }
}
