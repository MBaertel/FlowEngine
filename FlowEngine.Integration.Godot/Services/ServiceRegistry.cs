using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Integration.Godot.Services
{
    public class ServiceRegistry : Node
    {
        private Dictionary<string, Node> _services = new();

        public void RegisterService(string key,Node service) =>
            _services.Add(key, service);

        public T GetService<T>(string key) where T : Node =>
            (T)_services[key];
    }
}
