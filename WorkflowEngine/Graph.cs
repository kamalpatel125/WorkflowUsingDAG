namespace WorkflowUsingDAG.WorkflowEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Graph<T>
    {
        public Dictionary<T, Node<T>> Nodes { get; private set; } = new Dictionary<T, Node<T>>();

        public void AddNode(T value, string handlerTypeName, ExecutionMode mode)
        {
            if (!Nodes.ContainsKey(value))
            {
                Nodes[value] = new Node<T>(value, handlerTypeName, mode);
            }
        }

        public void AddDependency(T from, T to, DependencyType dependencyType)
        {
            if (Nodes.ContainsKey(from) && Nodes.ContainsKey(to))
            {
                Nodes[to].Dependencies.Add((Nodes[from], dependencyType));
            }
        }

        public IEnumerable<Node<T>> GetDependentNodes(T value)
        {
            return Nodes.Values.Where(node => node.Dependencies.Any(dep => dep.Node.Value.Equals(value)));
        }
    }

}