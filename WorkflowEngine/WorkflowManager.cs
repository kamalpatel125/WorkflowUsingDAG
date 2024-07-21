using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUsingDAG.WorkflowEngine
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public class WorkflowManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Graph<string> workflowGraph;
        private readonly ConcurrentDictionary<string, WorkflowInstance> workflowInstances;

        public WorkflowManager(IServiceProvider serviceProvider, Graph<string> workflowGraph)
        {
            this.serviceProvider = serviceProvider;
            this.workflowGraph = workflowGraph;
            workflowInstances = new ConcurrentDictionary<string, WorkflowInstance>();
        }

        public WorkflowInstance CreateWorkflowInstance()
        {
            var instance = new WorkflowInstance(serviceProvider, workflowGraph);
            workflowInstances.TryAdd(instance.Id, instance);
            return instance;
        }

        public WorkflowInstance GetWorkflowInstance(string id)
        {
            workflowInstances.TryGetValue(id, out var instance);
            return instance;
        }

        public async Task ExecuteWorkflowInstance(string id)
        {
            var instance = GetWorkflowInstance(id);
            if (instance != null)
            {
                await instance.Execute();
            }
            else
            {
                throw new ArgumentException("Workflow instance not found.");
            }
        }

        public async Task ExecuteManualTaskWithDelay(string id, string taskName, int delayInSeconds)
        {
            var instance = GetWorkflowInstance(id);
            if (instance != null)
            {
                await instance.ExecuteManualTaskWithDelay(taskName, delayInSeconds);
            }
            else
            {
                throw new ArgumentException("Workflow instance not found.");
            }
        }

        public bool IsWorkflowInstanceCompleted(string id)
        {
            var instance = GetWorkflowInstance(id);
            return instance?.IsCompleted ?? false;
        }
    }
}
