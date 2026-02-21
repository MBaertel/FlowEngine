using FlowEngine.Core.Events;
using FlowEngine.Core.Events.Bus;
using FlowEngine.Engine.Execution.Context;
using FlowEngine.Engine.Flows.Steps;
using FlowEngine.Integration.Services;
using FlowEngine.Integration.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Steps
{
    public class SystemStep<TIn, TOut, TBefore, TAfter>
        : IFlowStep<TIn, TOut>
        where TBefore : IBeforeEvent
        where TAfter : IAfterEvent
    {
        private ISystemRegistry _systemRegistry;
        private IConverterRegistry _converterRegistry;
        private IEventBus _eventBus;

        private ISystem<TIn, TOut> _system;
        public SystemStep(ISystemRegistry systemRegistry,IConverterRegistry converterRegistry, IEventBus eventBus)
        {
            _systemRegistry = systemRegistry;
            _eventBus = eventBus;
            _converterRegistry = converterRegistry;

            if (_systemRegistry.TryGetSystem<TIn, TOut>(out var sys))
                _system = sys;
            else
                throw new InvalidOperationException($"System with input type {typeof(TIn)} and output type {typeof(TOut)} not found.");
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
            if(_converterRegistry.TryGetConverter<TIn,TBefore>(out var converter))
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
            if (_converterRegistry.TryGetConverter<TOut, TAfter>(out var converter))
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
