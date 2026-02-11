using System;
using System.Collections.Generic;
using System.Text;
using FlowEngine.Engine.Flows.Definitions;
using FlowEngine.Engine.Flows.Graphs;
using FlowEngine.Engine.Flows.Graphs.Builder;
using FlowEngine.Engine.Flows.Orchestration;
using FlowEngine.Engine.Flows.Values;

namespace FlowEngine.Core.Commands
{
    public class CommandRouter : ICommandRouter
    {
        private readonly IFlowOrchestrator _flowOrchestrator;
        private readonly IFlowDefinitionRegistry _flowRegistry;

        private readonly Dictionary<Type, object> _bindings = new();

        public CommandRouter(IFlowOrchestrator flowOrchestrator, IFlowDefinitionRegistry flowDefinitionRegistry)
        {
            _flowOrchestrator = flowOrchestrator;
            _flowRegistry = flowDefinitionRegistry;
        }

        public void Register<TCommand, TCommandIn, TFlowIn>(Func<TCommandIn, TFlowIn>? adapter = null)
            where TCommand : ICommand
        {
            if (!typeof(TFlowIn).IsAssignableFrom(typeof(TCommandIn)) && adapter == null)
                throw new InvalidOperationException("Command Input and Flow Input dont match, adapter is required.");

            var binding = new CommandBinding<TCommandIn>(
                typeof(TCommand),
                CommandLambda.CommandTransformer(adapter)
            );
            _bindings.Add(typeof(TCommand), binding);
        }

        public async Task<TResult> RunCommand<TInput, TResult>(ICommand<TInput,TResult> command)
        {
            if (!_bindings.TryGetValue(command.GetType(), out var binding))
                throw new InvalidOperationException($"Command {command.GetType()} is not registered.");

            var flow = _flowRegistry.GetByName(command.FlowName);

            object input;
            if (_bindings[command.GetType()] is CommandBinding<TInput> typedBinding && typedBinding.InputAdapter != null)
                input = typedBinding.InputAdapter(command.InputValue);
            else
                input = command.InputValue;
            
            var result = await _flowOrchestrator.ExecuteFlowUntypedAsync(flow, input);
            if(result is TResult typedResult)
                return typedResult;

            throw new InvalidCastException($"Command returned value of type {result.GetType()}, expected {typeof(TResult)}");
        }
    }
}
