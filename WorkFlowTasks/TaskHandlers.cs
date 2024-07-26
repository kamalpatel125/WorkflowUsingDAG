using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Task1Handler : ITaskHandler
{
    public async Task<object> ExecuteAsync(object[] inputs)
    {
        Console.WriteLine("Task1 executed");
        return await Task.FromResult("Output from Task1");
    }
}

public class Task2Handler : ITaskHandler
{
    public async Task<object> ExecuteAsync(object[] inputs)
    {
        Console.WriteLine($"Task2 executed with input: {inputs[0]}");
        return await Task.FromResult("Output from Task2");
    }
}

public class Task3Handler : ITaskHandler
{
    public async Task<object> ExecuteAsync(object[] inputs)
    {
        Console.WriteLine($"Task3 executed with input: {inputs[0]}");
        return await Task.FromResult("Output from Task3");
    }
}

public class Task4Handler : ITaskHandler
{
    public async Task<object> ExecuteAsync(object[] inputs)
    {
        Console.WriteLine($"Task4 executed with input: {inputs[0]}");
        return await Task.FromResult("Output from Task4");
    }
}

public class Task5Handler : ITaskHandler
{
    public async Task<object> ExecuteAsync(object[] inputs)
    {
        Console.WriteLine($"Task5 executed with inputs from Task2 and Task4: {string.Join(", ", inputs)}");
        return await Task.FromResult("Output from Task5");
    }
}




