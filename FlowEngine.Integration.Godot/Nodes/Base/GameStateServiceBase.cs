using FlowEngine.Integration.Godot.Services;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Integration.Godot.Nodes.Base
{
    public abstract class GameStateServiceBase : Node
    {
        public virtual string ServiceKey => GetType().Name;

        public override void _Ready()
        {
            base._Ready();
            var registry = GetNode<ServiceRegistry>("../ServiceRegistry");
            if(registry == null)
            {
                GD.PushError($"Service Registry not found");
                return;
            }

            registry.RegisterService(ServiceKey, this);
        }
    }
}
