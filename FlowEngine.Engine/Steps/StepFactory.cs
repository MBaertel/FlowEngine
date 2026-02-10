using FlowEngine.Engine.Flows.Steps;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FlowEngine.Engine.Steps
{
    public class StepFactory : IStepFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, IFlowStep> _cache = new();

        public StepFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFlowStep Resolve(Type stepType)
        {
            if(!typeof(IFlowStep).IsAssignableFrom(stepType))
                throw new InvalidOperationException($"{stepType.Name} does not implement IFlowStep");

            return _cache.GetOrAdd(stepType,CreateInstance);
        }

        public IFlowStep Resolve<TStep>() where TStep : IFlowStep
        {
            return (IFlowStep)_cache.GetOrAdd(typeof(TStep),CreateInstance);
        }

        private IFlowStep CreateInstance(Type stepType)
        {
            var fromDi = _serviceProvider.GetService(stepType);
            if(fromDi is IFlowStep stepFromDi)
                return stepFromDi;

            var created = ActivatorUtilities.CreateInstance(_serviceProvider, stepType);

            if (created is not IFlowStep flowStep)
                throw new InvalidOperationException($"{stepType.Name} could not be created as IFlowStep");

            return flowStep;
        }
    }
}
