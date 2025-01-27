﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowUsingDAG.WorkflowEngine;
using WorkflowUsingDAG.WorkFlow;

namespace WorkflowUsingDAG
{
    class WorkflowOneGraph
    {
        public Graph<string> ConfigureWorkflowTasks()
        {
            var graph = new Graph<string>();

            // Define tasks and dependencies
            graph.AddNode("Task1", typeof(Task1Handler), ExecutionMode.Automatic);
            graph.AddNode("Task2", typeof(Task2Handler), ExecutionMode.Manual);
            graph.AddNode("Task3", typeof(Task3Handler), ExecutionMode.Automatic);
            graph.AddNode("Task4", typeof(Task4Handler), ExecutionMode.Automatic);
            graph.AddNode("Task5", typeof(Task5Handler), ExecutionMode.Automatic);

            graph.AddDependency("Task1", "Task2");
            graph.AddDependency("Task1", "Task3");
            graph.AddDependency("Task3", "Task4");
            graph.AddDependency("Task2", "Task5");
            graph.AddDependency("Task4", "Task5");

         /*
         *************************************************************************************************************************
                             Workflow Graph Setup  (A - Automatic Task, M - Manual task)
         *************************************************************************************************************************                    

                                                        Task1 (Start Workflow Node) (A)
                                                            |                       |    
                                                            V                       V
                                            Task2 (Depends on Task1)(M)        Task3 (Depends on Task1) (A)
                                                    |                                    |   
                                                    |                                    V
                                                    |                          Task4 (Depends on Task3) (A)
                                                    |                                    |    
                                                    V                                    V
                                    Task5 (Depends on Task2 and Task4) (End Workflow Node) (A)
         **************************************************************************************************************************
        */

            return graph;
        }
    }
}
