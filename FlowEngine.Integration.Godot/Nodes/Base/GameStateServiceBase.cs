using FlowEngine.Integration.Godot;
using FlowEngine.Integration.Services;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Integration.Godot.Nodes.Base
{
    [GlobalClass]
    public abstract class GameStateServiceBase : Node, IGameStateService
    {
        public abstract string Name { get; }

        public override void _Ready()
        {
            base._Ready();
            var engine = GetNode<FlowEngine>("../ServiceRegistry");
            if(engine == null)
            {
                GD.PushError($"Service Registry not found");
                return;
            }

            engine.RegisterService(this);
        }
    }
}
