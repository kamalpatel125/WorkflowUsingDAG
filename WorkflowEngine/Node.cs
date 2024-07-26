using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WorkflowUsingDAG.WorkflowEngine;

namespace WorkflowUsingDAG.WorkflowEngine
{
    public enum ExecutionMode
    {
        Automatic,
        Manual
    }
    public enum DependencyType
    {
        And,
        Or
    }

    public class Node<T>
    {
        public T Value { get; set; }
        public List<(Node<T> Node, DependencyType DependencyType)> Dependencies { get; set; }
        public string HandlerTypeName { get; set; }
        public ExecutionMode Mode { get; set; }

        public Node(T value, string handlerTypeName, ExecutionMode mode)
        {
            Value = value;
            HandlerTypeName = handlerTypeName;
            Mode = mode;
            Dependencies = new List<(Node<T> Node, DependencyType DependencyType)>();
        }

        public async Task<object> Execute(IServiceProvider serviceProvider, object[] inputs)
        {
            var handlerType = ResolveHandlerType(HandlerTypeName);
            var handler = (ITaskHandler)serviceProvider.GetRequiredService(handlerType);
            return await handler.ExecuteAsync(inputs);
        }

        private Type ResolveHandlerType(string handlerTypeName)
        {
            return Assembly.GetExecutingAssembly().GetType(handlerTypeName)
                   ?? throw new InvalidOperationException($"Handler type '{handlerTypeName}' not found.");
        }
    }

}