using FlowEngine.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Converters
{
    public interface IEventConverter
    {
        Type ItemType { get; }
        Type EventType { get; }
    }


    public interface IEventConverter<TItem,TEvent>
        : IEventConverter
        where TEvent : IFlowEvent
    {
        TEvent Convert(TItem item);
        TItem ConvertBack(TEvent evt);
    }
}
