using FlowEngine.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Converters
{
    public interface IConverterRegistry
    {
        bool TryGetConverter(Type itemType,out IEventConverter converter);
        bool TryGetConverter<TItem, TEvent>(out IEventConverter<TItem,TEvent> converter)
            where TEvent : IFlowEvent;

        void Register(IEventConverter converter);
    }
}
