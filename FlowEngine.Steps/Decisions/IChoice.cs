using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Integration.Decisions
{
    public interface IChoice
    {
        Guid ChoiceId { get; }
    }

    public interface IChoiceResult
    {

    }
}
