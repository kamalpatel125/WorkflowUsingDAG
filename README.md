# Workflow Engine Using Directed Acyclic Graph(DAG) 

- Utilize Directed Acyclic Graph (DAG) to define workflow steps, including graph nodes and their dependencies.
- Ensure correct order of node dependencies using topological sorting.
- Define each step node with a handler interface.
- Specify each workflow step (task) as either automatic or manual.
- Execute automatic steps by invoking the appropriate handler.
- Require intervention for manual steps, which may involve user input or interaction with other systems.
- Ensure each step receives inputs derived from the outputs of all preceding tasks it depends on.
- Prevent execution of any step until all its dependent tasks are completed, whether automatic or manual.
- Effectively manage workflow instances to monitor progress and verify completion.
- Include examples to demonstrate the performance and functionality of the code.
- Highlight that this approach results in a loosely coupled and easily modifiable workflow.
