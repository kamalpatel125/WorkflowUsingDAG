using Microsoft.Extensions.DependencyInjection;
using WorkflowUsingDAG.WorkFlow;
using WorkflowUsingDAG.Workflows.WorkflowOne;
using WorkflowUsingDAG.Workflows.WorkflowTwo;

namespace WorkflowUsingDAG
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Task1Handler>()
                .AddSingleton<Task2Handler>()
                .AddSingleton<Task3Handler>()
                .AddSingleton<Task4Handler>()
                .AddSingleton<Task5Handler>()
                .BuildServiceProvider();

            var WorkflowOneInstance1 = new WorkflowOne(serviceProvider);
            await WorkflowOneInstance1.ConfigureAndExecuteWorkflow();

            var WorkflowOneInstance2 = new WorkflowOne(serviceProvider);
            await WorkflowOneInstance2.ConfigureAndExecuteWorkflow();


            //var WorkflowTwoInstance1 = new WorkflowTwo(serviceProvider);
            //await WorkflowTwoInstance1.ConfigureAndExecuteWorkflow();

            //var WorkflowTwoInstance2 = new WorkflowTwo(serviceProvider);
            //await WorkflowTwoInstance2.ConfigureAndExecuteWorkflow();
            
            //var WorkflowTwoInstance3 = new WorkflowTwo(serviceProvider);
            //await WorkflowTwoInstance3.ConfigureAndExecuteWorkflow();

        }
    }

}