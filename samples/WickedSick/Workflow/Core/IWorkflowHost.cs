using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WickedSick.Workflow.Core
{
    [ServiceContract(Namespace = "http://WickedSick.Workflow.Core")]
    public interface IWorkflowHost
    {
        [OperationContract]
        void StartWorkflow(string ApplicationName, string WorkflowName, Dictionary<string, object> Parameters);
    }
}
