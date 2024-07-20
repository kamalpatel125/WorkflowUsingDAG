﻿
namespace WorkflowUsingDAG
{
   class Graph<T>
    {
        public Dictionary<T, Node<T>> Nodes { get; set; }

        public Graph()
        {
            Nodes = new Dictionary<T, Node<T>>();
        }

        public void AddNode(T value, Func<object[], object> handler, ExecutionMode mode)
        {
            if (!Nodes.ContainsKey(value))
            {
                Nodes[value] = new Node<T>(value, handler, mode);
            }
        }

        public void AddDependency(T from, T to)
        {
            if (!Nodes.ContainsKey(from) || !Nodes.ContainsKey(to))
            {
                throw new ArgumentException("Both nodes must be added before adding a dependency.");
            }

            Nodes[to].Dependencies.Add(Nodes[from]);
        }

        public List<Node<T>> GetDependentNodes(T nodeValue)
        {
            return Nodes.Values.Where(n => n.Dependencies.Any(d => d.Value.Equals(nodeValue))).ToList();
        }
    }
}