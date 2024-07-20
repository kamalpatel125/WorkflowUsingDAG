using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUsingDAG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Workflow
    {
        private readonly IServiceProvider serviceProvider;
        private Graph<string> graph;
        private Queue<string> manualTaskQueue;
        private Dictionary<string, object> outputs;
        private Dictionary<string, List<string>> dependentTasks;
        private HashSet<string> executedTasks;
        private Dictionary<string, List<object>> pendingInputs;

        public Workflow(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            graph = new Graph<string>();
            manualTaskQueue = new Queue<string>();
            outputs = new Dictionary<string, object>();
            dependentTasks = new Dictionary<string, List<string>>();
            executedTasks = new HashSet<string>();
            pendingInputs = new Dictionary<string, List<object>>();
        }

        public void AddTask(string task, Type handlerType, ExecutionMode mode)
        {
            graph.AddNode(task, handlerType, mode);
            dependentTasks[task] = new List<string>();
            pendingInputs[task] = new List<object>();
        }

        public void AddDependency(string task, string dependentTask)
        {
            graph.AddDependency(task, dependentTask);
            dependentTasks[dependentTask].Add(task);
        }

        public async Task Execute()
        {
            var sortedTasks = TopologicalSorter<string>.Sort(graph);

            foreach (var taskName in sortedTasks)
            {
                if (graph.Nodes[taskName].Dependencies.Count == 0)
                {
                    await ExecuteTask(taskName, new object[0]);
                }
            }
        }

        private async Task ExecuteTask(string taskName, object[] inputs)
        {
            if (graph.Nodes.ContainsKey(taskName) && !executedTasks.Contains(taskName))
            {
                var task = graph.Nodes[taskName];
                if (task.Mode == ExecutionMode.Automatic)
                {
                    Console.WriteLine($"Executing task: {task.Value}");
                    var output = await task.Execute(serviceProvider, inputs);
                    outputs[taskName] = output;
                    executedTasks.Add(taskName);
                    await ExecuteDependentTasks(taskName, output);
                }
                else
                {
                    Console.WriteLine($"Task {task.Value} is set to manual execution. Waiting for manual intervention.");
                    manualTaskQueue.Enqueue(taskName);
                }
            }
            else
            {
                Console.WriteLine($"Task {taskName} does not exist or has already been executed.");
            }
        }

        private async Task ExecuteDependentTasks(string taskName, object output)
        {
            var dependentNodes = graph.GetDependentNodes(taskName);
            foreach (var dependentNode in dependentNodes)
            {
                if (dependentNode.Dependencies.All(dep => executedTasks.Contains(dep.Value)))
                {
                    var inputs = dependentNode.Dependencies.Select(dep => outputs[dep.Value]).ToArray();
                    await ExecuteTask(dependentNode.Value, inputs);
                }
            }
        }

        public async Task ExecuteManualTaskWithDelay(string taskName, int delayInSeconds)
        {
            await Task.Delay(delayInSeconds * 1000);
            await ExecuteManualTask(taskName);
        }

        private async Task ExecuteManualTask(string taskName)
        {
            if (graph.Nodes.ContainsKey(taskName))
            {
                var task = graph.Nodes[taskName];
                if (task.Mode == ExecutionMode.Manual && manualTaskQueue.Contains(taskName))
                {
                    Console.WriteLine($"Manually executing task: {task.Value}");
                    var inputs = task.Dependencies.Select(dep => outputs[dep.Value]).ToArray();
                    var output = await task.Execute(serviceProvider, inputs);
                    outputs[taskName] = output;
                    executedTasks.Add(taskName);
                    manualTaskQueue.Dequeue();
                    await ExecuteDependentTasks(taskName, output);
                }
                else
                {
                    Console.WriteLine($"Task {task.Value} is not set to manual execution or not in queue.");
                }
            }
            else
            {
                Console.WriteLine($"Task {taskName} does not exist.");
            }
        }
    }
}
