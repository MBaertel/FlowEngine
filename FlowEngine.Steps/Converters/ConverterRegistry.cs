using FlowEngine.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FlowEngine.Integration.Converters
{
    public class ConverterRegistry : IConverterRegistry
    {
        private readonly Dictionary<Type, IEventConverter> _converters = new();

        public ConverterRegistry(IServiceProvider serviceProvider)
        {
            var converters = serviceProvider.GetServices<IEventConverter>();
            foreach (var converter in converters)
            {
                _converters[converter.ItemType] = converter;   
            }
        }

        public void Register(IEventConverter converter) =>
            _converters[converter.ItemType] = converter;

        public bool TryGetConverter(Type itemType, out IEventConverter converter)
        {
            if (_converters.TryGetValue(itemType, out converter))
                return true;
            return false;
        }

        public bool TryGetConverter<TItem, TEvent>(out IEventConverter<TItem, TEvent> converter) where TEvent : IFlowEvent
        {
            if(_converters.TryGetValue(typeof(TItem),out var untypedConverter))
            {
                if(untypedConverter is IEventConverter<TItem, TEvent> typedConverter)
                {
                    converter = typedConverter;
                    return true;
                }
            }
            converter = null;
            return false;
        }
    }
}
