using System;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Values
{
    public readonly record struct SubflowCallKey(Guid DefinitionId,int CallIndex);
}
