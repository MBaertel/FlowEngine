using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Integration.Services
{
    public interface IServiceRegistry
    {
        void RegisterService(IGameStateService gameStateService);
        bool TryGetService<TService>(out TService service) 
            where TService : IGameStateService;
        bool TryGetService(string name, out IGameStateService service);
    }
}
