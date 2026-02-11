using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Definitions
{
    public abstract class FlowDefinitionBase<TInput,TOutput> : IFlowDefinition<TInput,TOutput>
    {
        public Type InputType => typeof(TInput);
        public Type OutputType => typeof(TOutput);

        public virtual Guid Id { get; } = Guid.NewGuid();
        public abstract string Name { get; }

        public FlowGraph Flow { get; private set; }

        public FlowDefinitionBase()
        {
            Flow = BuildFlow();
        }

        protected abstract FlowGraph BuildFlow();
    }
}
