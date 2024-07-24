using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUsingDAG.WorkflowEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public class WorkflowInstance
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Graph<string> graph;
        private readonly Queue<string> manualTaskQueue;
        private readonly Dictionary<string, object> outputs;
        private readonly Dictionary<string, List<string>> dependentTasks;
        private readonly HashSet<string> executedTasks;
        private readonly Dictionary<string, List<object>> pendingInputs;

        public string Id { get; }
        public bool IsCompleted { get; private set; }

        public WorkflowInstance(IServiceProvider serviceProvider, Graph<string> graph)
        {
            this.serviceProvider = serviceProvider;
            this.graph = graph;
            manualTaskQueue = new Queue<string>();
            outputs = new Dictionary<string, object>();
            dependentTasks = new Dictionary<string, List<string>>();
            executedTasks = new HashSet<string>();
            pendingInputs = new Dictionary<string, List<object>>();
            Id = Guid.NewGuid().ToString();
            IsCompleted = false;

            foreach (var node in graph.Nodes.Keys)
            {
                dependentTasks[node] = new List<string>();
                pendingInputs[node] = new List<object>();
            }
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

            CheckCompletion();
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
                pendingInputs[dependentNode.Value].Add(output);

                bool canExecute = true;
                var inputs = new List<object>();

                foreach (var (dependency, dependencyType) in dependentNode.Dependencies)
                {
                    if (dependencyType == DependencyType.And)
                    {
                        if (!executedTasks.Contains(dependency.Value))
                        {
                            canExecute = false;
                            break;
                        }
                    }
                    else if (dependencyType == DependencyType.Or)
                    {
                        if (executedTasks.Contains(dependency.Value))
                        {
                            inputs.Add(outputs[dependency.Value]);
                            canExecute = true;
                            break;
                        }
                        else
                        {
                            canExecute = false;
                        }
                    }
                    inputs.Add(outputs[dependency.Value]);
                }

                if (canExecute)
                {
                    await ExecuteTask(dependentNode.Value, inputs.ToArray());
                }
            }

            CheckCompletion();
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
                    var inputs = task.Dependencies.Select(dep => outputs[dep.Node.Value]).ToArray();
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

            CheckCompletion();
        }

        private void CheckCompletion()
        {
            IsCompleted = graph.Nodes.Keys.All(task => executedTasks.Contains(task));
        }
    }


}
