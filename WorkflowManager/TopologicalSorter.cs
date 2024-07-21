namespace WorkflowUsingDAG.WorkflowManager
{
    class TopologicalSorter<T>
    {
        public static List<T> Sort(Graph<T> graph)
        {
            var sortedList = new List<T>();
            var visited = new HashSet<T>();
            var tempMarked = new HashSet<T>();

            foreach (var node in graph.Nodes.Values)
            {
                if (!visited.Contains(node.Value))
                {
                    Visit(node, visited, tempMarked, sortedList);
                }
            }

            return sortedList;
        }

        private static void Visit(Node<T> node, HashSet<T> visited, HashSet<T> tempMarked, List<T> sortedList)
        {
            if (tempMarked.Contains(node.Value))
            {
                throw new InvalidOperationException("The graph contains a cycle.");
            }

            if (!visited.Contains(node.Value))
            {
                tempMarked.Add(node.Value);

                foreach (var dependency in node.Dependencies)
                {
                    Visit(dependency, visited, tempMarked, sortedList);
                }

                tempMarked.Remove(node.Value);
                visited.Add(node.Value);
                sortedList.Add(node.Value);
            }
        }
    }
}