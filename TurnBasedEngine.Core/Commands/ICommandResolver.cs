using System;
using System.Collections.Generic;
using System.Text;

namespace TurnBasedEngine.Core.Commands
{
    public interface ICommandResolver
    {
        FlowStartRequest Resolve(ICommand command);
    }
}
