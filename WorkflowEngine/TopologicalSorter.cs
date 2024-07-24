namespace WorkflowUsingDAG.WorkflowEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TopologicalSorter<T>
    {
        public static List<T> Sort(Graph<T> graph)
        {
            var sorted = new List<T>();
            var visited = new HashSet<T>();
            var visiting = new HashSet<T>();

            foreach (var node in graph.Nodes.Values)
            {
                if (!visited.Contains(node.Value))
                {
                    if (!Visit(node, visited, visiting, sorted))
                    {
                        throw new InvalidOperationException("Graph has a cycle.");
                    }
                }
            }

            return sorted;
        }

        private static bool Visit(Node<T> node, HashSet<T> visited, HashSet<T> visiting, List<T> sorted)
        {
            if (visiting.Contains(node.Value))
            {
                // Node is in the visiting set, which means we have a cycle.
                return false;
            }

            if (!visited.Contains(node.Value))
            {
                // Mark the node as visiting
                visiting.Add(node.Value);

                foreach (var dependency in node.Dependencies)
                {
                    if (!Visit(dependency.Node, visited, visiting, sorted))
                    {
                        return false;
                    }
                }

                // Mark the node as visited
                visiting.Remove(node.Value);
                visited.Add(node.Value);
                sorted.Add(node.Value);
            }

            return true;
        }
    }

}