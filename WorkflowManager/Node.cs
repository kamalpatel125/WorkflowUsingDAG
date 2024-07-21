using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowUsingDAG.WorkFlow;

namespace WorkflowUsingDAG.WorkflowManager
{
    public enum ExecutionMode
    {
        Automatic,
        Manual
    }
    public class Node<T>
    {
        public T Value { get; set; }
        public List<Node<T>> Dependencies { get; set; }
        public Type HandlerType { get; set; }
        public ExecutionMode Mode { get; set; }

        public Node(T value, Type handlerType, ExecutionMode mode)
        {
            Value = value;
            HandlerType = handlerType;
            Mode = mode;
            Dependencies = new List<Node<T>>();
        }

        public async Task<object> Execute(IServiceProvider serviceProvider, object[] inputs)
        {
            var handler = (ITaskHandler)serviceProvider.GetRequiredService(HandlerType);
            return await handler.ExecuteAsync(inputs);
        }
    }
}