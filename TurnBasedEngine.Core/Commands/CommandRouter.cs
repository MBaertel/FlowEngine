using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Core.Commands
{
    public class CommandRouter : ICommandRouter
    {
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly IFlowDefinitionRegistry _flowRegistry;

        private readonly Dictionary<Type, CommandBinding> _bindings = new();

        public CommandRouter(IFlowOrchestrator flowOrchestrator, IFlowDefinitionRegistry flowDefinitionRegistry)
        {
            _flowOrchestrator = flowOrchestrator;
            _flowRegistry = flowDefinitionRegistry;
        }

        public async Task<TResult> RunCommand<TResult>(ICommand command)
        {
            var binding = ResolveBinding(command);
            var flow = _flowRegistry.GetByName(command.FlowName);
            
            var input = binding.InputAdapter(command.InputValue);

            var result = await _flowOrchestrator.ExecuteFlowAsync(flow, input);

            return result.Unwrap<TResult>();
        }

        public void Register<TCommand, TCommandIn, TFlowIn>(Func<TCommandIn, TFlowIn> adapter)
            where TCommand : ICommand
            where TFlowIn : notnull
        {
            FlowValue Wrapper(object? obj)
            {
                if (obj is not TCommandIn input)
                    throw new InvalidOperationException($"Command value must be {typeof(TCommandIn).Name}");

                var result = adapter(input);
                
                return FlowValue.Wrap(result);
            }

            _bindings[typeof(TCommand)] = new CommandBinding(typeof(TCommand),Wrapper);
        }

        private CommandBinding ResolveBinding(ICommand command)
        {
            if (!_bindings.TryGetValue(command.GetType(), out var binding))
                return CommandBinding.Default;
            return binding;
        }
    }
}
