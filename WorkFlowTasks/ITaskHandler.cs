using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowUsingDAG.WorkFlow
{
    public interface ITaskHandler
    {
        Task<object> ExecuteAsync(object[] inputs);
    }
}
