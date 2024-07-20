using Microsoft.Extensions.DependencyInjection;

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

            var workflowDefinition1 = new WorkflowDefinition1(serviceProvider);
            await workflowDefinition1.ConfigureAndExecuteWorkflow();

            var workflowDefinition2 = new WorkflowDefinition2(serviceProvider);
            await workflowDefinition2.ConfigureAndExecuteWorkflow();
        }
    }

}