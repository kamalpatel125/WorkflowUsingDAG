﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowUsingDAG.WorkFlow;
using WorkflowUsingDAG.WorkflowEngine;

namespace WorkflowUsingDAG.Workflows.WorkflowTwo
{
    class WorkflowTwo
    {
        private readonly IServiceProvider serviceProvider;
        public WorkflowTwo(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task ConfigureAndExecuteWorkflow()
        {
            var configureWorkflowTasks = new WorkflowTwoGraph();
            var workflowGraph = configureWorkflowTasks.ConfigureWorkflowTasks();
            var workflowManager = new WorkflowEngine.WorkflowManager(serviceProvider, workflowGraph);
            var instance = workflowManager.CreateWorkflowInstance();

            Console.WriteLine("****************** WorkflowDefinition2 execution start ***************************");

            // Automatically execute the automatic task
            await workflowManager.ExecuteWorkflowInstance(instance.Id);

            // Manually execute the manual task after a delay
            await workflowManager.ExecuteManualTaskWithDelay(instance.Id, "Task2", 5);
            // Execution will automatically continue after the manual task

            // Check if the workflow instance has completed
            var isCompleted = workflowManager.IsWorkflowInstanceCompleted(instance.Id);
            Console.WriteLine($"Workflow instance {instance.Id} completed: {isCompleted}");

            Console.WriteLine("****************** WorkflowDefinition2 execution finish ***************************");
            Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////");
            Console.WriteLine("///////////////////////////////////////////////////////////////////////////////////");
        }
    }
}
