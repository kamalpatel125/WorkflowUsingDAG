using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowUsingDAG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var workflow = new Workflow();

            workflow.AddTask("Task1", (inputs) =>
            {
                Console.WriteLine("Task1 executed");
                return "Output from Task1";
            }, ExecutionMode.Automatic);

            workflow.AddTask("Task2", (inputs) =>
            {
                Console.WriteLine($"Task2 executed with input: {inputs[0]}");
                return "Output from Task2";
            }, ExecutionMode.Manual);

            workflow.AddTask("Task3", (inputs) =>
            {
                Console.WriteLine($"Task3 executed with input: {inputs[0]}");
                return "Output from Task3";
            }, ExecutionMode.Automatic);

            workflow.AddTask("Task4", (inputs) =>
            {
                Console.WriteLine($"Task4 executed with input: {inputs[0]}");
                return "Output from Task4";
            }, ExecutionMode.Automatic);

            workflow.AddTask("Task5", (inputs) =>
            {
                Console.WriteLine($"Task5 executed with inputs from Task2 and Task4: {string.Join(", ", inputs)}");
                return "Output from Task5";
            }, ExecutionMode.Automatic);

            workflow.AddDependency("Task1", "Task2");
            workflow.AddDependency("Task1", "Task3");
            workflow.AddDependency("Task3", "Task4");
            workflow.AddDependency("Task2", "Task5");
            workflow.AddDependency("Task4", "Task5");

            workflow.Execute();

            // Manually execute the manual task after a delay
            await workflow.ExecuteManualTaskWithDelay("Task2", 5);
            // Execution will automatically continue after the manual task
        }
    }
}