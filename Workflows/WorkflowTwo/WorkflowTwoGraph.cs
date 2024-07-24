using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowUsingDAG.WorkflowEngine;
using WorkflowUsingDAG.WorkFlow;

namespace WorkflowUsingDAG
{
    class WorkflowTwoGraph
    {
        public Graph<string> ConfigureWorkflowTasks()
        {
            var graph = new Graph<string>();

            // Define tasks and dependencies
            graph.AddNode("Task1", typeof(Task1Handler), ExecutionMode.Automatic);
            graph.AddNode("Task2", typeof(Task2Handler), ExecutionMode.Automatic);
            graph.AddNode("Task3", typeof(Task3Handler), ExecutionMode.Automatic);
            
            graph.AddDependency("Task1", "Task2", DependencyType.And);
            graph.AddDependency("Task1", "Task3", DependencyType.And);

            /*
*************************************************************************************************************************
                    Workflow Graph Setup  (A - Automatic Task, M - Manual task)
*************************************************************************************************************************                    

                                               Task1 (Start Workflow Node) (A)
                                                   |                       |    
                                                   V                       V
                                   Task2 (Depends on Task1)(A)        Task3 (Depends on Task1) (A)
**************************************************************************************************************************
*/

            return graph;
        }
    }
}
