using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowUsingDAG.WorkFlow;
using WorkflowUsingDAG.WorkflowManager;

namespace WorkflowUsingDAG.Workflows.WorkflowOne
{
    class WorkflowOne
    {
        private readonly IServiceProvider serviceProvider;
        public WorkflowOne(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task ConfigureAndExecuteWorkflow()
        {
            var configureWorkflowTasks = new WorkflowOneGraph();
            var workflowGraph = configureWorkflowTasks.ConfigureWorkflowTasks();
            var workflowManager = new WorkflowManager.WorkflowManager(serviceProvider, workflowGraph);
            var instance = workflowManager.CreateWorkflowInstance();


            Console.WriteLine("****************** WorkflowDefinition1 execution start ***************************");

            // Automatically execute the automatic task
            await workflowManager.ExecuteWorkflowInstance(instance.Id);

            // Manually execute the manual task after a delay
            await workflowManager.ExecuteManualTaskWithDelay(instance.Id, "Task2", 5);
            // Execution will automatically continue after the manual task

            // Check if the workflow instance has completed
            var isCompleted = workflowManager.IsWorkflowInstanceCompleted(instance.Id);
            Console.WriteLine($"Workflow instance {instance.Id} completed: {isCompleted}");

            Console.WriteLine("****************** WorkflowDefinition1 execution finish ***************************");
            Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////");
            Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////");
        }
    }
}
