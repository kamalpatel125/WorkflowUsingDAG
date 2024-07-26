using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkflowUsingDAG.Workflows.WorkflowOne;
using WorkflowUsingDAG.Workflows.WorkflowTwo;
using WorkflowUsingDAG.WorkflowEngine;

namespace WorkflowUsingDAG
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            // Dynamically register all ITaskHandler implementations
            RegisterTaskHandlers(serviceCollection, Assembly.GetExecutingAssembly());
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            var WorkflowOneInstance1 = new WorkflowOne(serviceProvider);
            await WorkflowOneInstance1.ConfigureAndExecuteWorkflow();

            var WorkflowOneInstance2 = new WorkflowOne(serviceProvider);
            await WorkflowOneInstance2.ConfigureAndExecuteWorkflow();


            var WorkflowTwoInstance1 = new WorkflowTwo(serviceProvider);
            await WorkflowTwoInstance1.ConfigureAndExecuteWorkflow();

            var WorkflowTwoInstance2 = new WorkflowTwo(serviceProvider);
            await WorkflowTwoInstance2.ConfigureAndExecuteWorkflow();
            
            var WorkflowTwoInstance3 = new WorkflowTwo(serviceProvider);
            await WorkflowTwoInstance3.ConfigureAndExecuteWorkflow();

        }

        private static void RegisterTaskHandlers(IServiceCollection services, Assembly assembly)
        {
            var taskHandlerType = typeof(ITaskHandler);
            var implementations = assembly.GetTypes()
                .Where(t => taskHandlerType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var implementation in implementations)
            {
                services.AddTransient(implementation); 
            }
        }
    }

}