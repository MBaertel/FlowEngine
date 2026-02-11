using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Steps;
using FlowEngine.Engine.Values;
using FlowEngine.Integration.Systems;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FlowEngine.Integration.Steps
{
    public sealed class SystemStep<TSystem,TIn,TOut> : FlowStepBase<TIn,TOut>
        where TSystem : ISystem<TIn,TOut>
    {
        private readonly ISystem<TIn,TOut> _system;

        public SystemStep(TSystem system)
        {
            _system = system;
        }

        public override async Task<TOut> ExecuteAsync(IFlowContext ctx, TIn input)
        {
            await _system.RunAsync(input);
        }

        public override void Undo(TIn input, TOut output)
        {
            throw new NotImplementedException();
        }
    }
}
