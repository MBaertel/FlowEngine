using FlowEngine.Integration.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Integration.Services
{
    public class ServiceRegistry : IServiceRegistry
    {
        private readonly Dictionary<Type,IGameStateService> _servicesByType = new();
        private readonly Dictionary<string, IGameStateService> _servicesByName = new();

        public void RegisterService(IGameStateService gameStateService)
        {
            _servicesByType[gameStateService.GetType()] = gameStateService;
            _servicesByName[gameStateService.Name] = gameStateService;  
        }

        public bool TryGetService<TService>(out TService service)
            where TService : IGameStateService
        {
            
            if (_servicesByType.TryGetValue(typeof(TService), out var resultService) 
                && resultService is TService typedService)
            {
                service = typedService;
                return true;
            }
            service = default;
            return false;
        }

        public bool TryGetService(string name, out IGameStateService service) =>
            _servicesByName.TryGetValue(name,out service);
    }
}
