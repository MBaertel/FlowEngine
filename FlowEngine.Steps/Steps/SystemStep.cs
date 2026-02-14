using FlowEngine.Core.Events;
using FlowEngine.Core.Events.Bus;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Integration.Converters;
using FlowEngine.Integration.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Steps
{
    public class SystemStep<TIn, TOut,TSystem, TBefore, TAfter>
        : IFlowStep<TIn, TOut>
        where TSystem : ISystem<TIn,TOut>
        where TBefore : IBeforeEvent
        where TAfter : IAfterEvent
    {
        private TSystem _system;
        private IConverterRegistry _registry;
        private IEventBus _eventBus;


        public SystemStep(TSystem system,IConverterRegistry converterRegistry, IEventBus eventBus)
        {
            _system = system;
            _eventBus = eventBus;
            _registry = converterRegistry;
        }

        public async Task<TOut> ExecuteAsync(IStepContext ctx, TIn input)
        {
            var mutatedInput = await RaiseBeforeEvent(input);
            var output = await _system.RunAsync(mutatedInput);
            await RaiseAfterEvent(output);
            return output;
        }

        private async Task<TIn> RaiseBeforeEvent(TIn input)
        {
            if(_registry.TryGetConverter<TIn,TBefore>(out var converter))
            {
                var evt = converter.Convert(input);
                await _eventBus.PublishAsync(evt);
                return converter.ConvertBack(evt);
            }
            else
                return input;
        }

        private async Task<TOut> RaiseAfterEvent(TOut output)
        {
            if (_registry.TryGetConverter<TOut, TAfter>(out var converter))
            {
                var evt = converter.Convert(output);
                await _eventBus.PublishAsync(evt);
                return converter.ConvertBack(evt);
            }
            else
                return output;
        }

        public async Task<object> ExecuteAsyncUntyped(IStepContext ctx, object input)
        {
            if (input is TIn typedInput)
                return await ExecuteAsync(ctx, typedInput);
            throw new InvalidCastException($"Input was {input.GetType()}, expected {typeof(TIn)}");
        }

        public void Undo(TIn input, TOut output)
        {
            throw new NotImplementedException();
        }

        public void UndoUntyped(object input, object output)
        {
            throw new NotImplementedException();
        }
    }
}
