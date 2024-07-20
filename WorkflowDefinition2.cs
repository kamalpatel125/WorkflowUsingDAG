using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUsingDAG
{
    class WorkflowDefinition2
    {
        private readonly IServiceProvider serviceProvider;
        public WorkflowDefinition2(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task ConfigureAndExecuteWorkflow()
        {
            var workflow = new Workflow(serviceProvider);

            workflow.AddTask("Task1", typeof(Task1Handler), ExecutionMode.Automatic);
            workflow.AddTask("Task2", typeof(Task2Handler), ExecutionMode.Automatic);
            workflow.AddTask("Task3", typeof(Task3Handler), ExecutionMode.Automatic);

            workflow.AddDependency("Task1", "Task2");
            workflow.AddDependency("Task1", "Task3");
            
            Console.WriteLine("****************** WorkflowDefinition2 execution start ***************************");

            await workflow.Execute();
            
            Console.WriteLine("****************** WorkflowDefinition2 execution finish ***************************");
            Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////");

        }
    }
}

