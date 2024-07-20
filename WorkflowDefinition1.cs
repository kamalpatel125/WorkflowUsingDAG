using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUsingDAG
{
    class WorkflowDefinition1
    {
        private readonly IServiceProvider serviceProvider;
        public WorkflowDefinition1(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task ConfigureAndExecuteWorkflow()
        {
            var workflow = new Workflow(serviceProvider);

            workflow.AddTask("Task1", typeof(Task1Handler), ExecutionMode.Automatic);
            workflow.AddTask("Task2", typeof(Task2Handler), ExecutionMode.Manual);
            workflow.AddTask("Task3", typeof(Task3Handler), ExecutionMode.Automatic);
            workflow.AddTask("Task4", typeof(Task4Handler), ExecutionMode.Automatic);
            workflow.AddTask("Task5", typeof(Task5Handler), ExecutionMode.Automatic);

            workflow.AddDependency("Task1", "Task2");
            workflow.AddDependency("Task1", "Task3");
            workflow.AddDependency("Task3", "Task4");
            workflow.AddDependency("Task2", "Task5");
            workflow.AddDependency("Task4", "Task5");

            Console.WriteLine("****************** WorkflowDefinition1 execution start ***************************");

            await workflow.Execute();
            // Manually execute the manual task after a delay
            await workflow.ExecuteManualTaskWithDelay("Task2", 5);
            // Execution will automatically continue after the manual task

            Console.WriteLine("****************** WorkflowDefinition1 execution finish ***************************");
            Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////");
        }
    }
}
