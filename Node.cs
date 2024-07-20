
namespace WorkflowUsingDAG
{
    enum ExecutionMode
    {
        Automatic,
        Manual
    }

    class Node<T>
    {
        public T Value { get; set; }
        public List<Node<T>> Dependencies { get; set; }
        public Func<object[], object> Handler { get; set; }
        public ExecutionMode Mode { get; set; }

        public Node(T value, Func<object[], object> handler, ExecutionMode mode)
        {
            Value = value;
            Handler = handler;
            Mode = mode;
            Dependencies = new List<Node<T>>();
        }

        public object Execute(object[] inputs)
        {
            return Handler?.Invoke(inputs);
        }
    }
}