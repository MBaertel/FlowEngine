using FlowEngine.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Converters
{
    public class LambdaConverter<TItem, TEvent>
        : IEventConverter<TItem, TEvent>
        where TEvent : IFlowEvent
    {
        public Type ItemType => typeof(TItem);
        public Type EventType => typeof(TEvent);

        private readonly Func<TItem, TEvent> _convert;
        private readonly Func<TEvent,TItem> _convertBack;

        public LambdaConverter(
            Func<TItem,TEvent> convertFunc,
            Func<TEvent,TItem> convertBackFunc)
        {
            _convert = convertFunc;
            _convertBack = convertBackFunc;
        }

        public TEvent Convert(TItem item) =>
            _convert(item);

        public TItem ConvertBack(TEvent evt) =>
            _convertBack(evt);
    }
}
