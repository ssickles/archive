using System.ServiceModel;
using System.Collections.Generic;

namespace CdcSoftware.Workflow.Core
{
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                       IncludeExceptionDetailInFaults = true)]
	public class WorkflowRemote
	{
        private WorkflowRuntimeManager _manager;
        
        public WorkflowRemote(WorkflowRuntimeManager Manager)
        {
            _manager = Manager;
        }

        #region IWorkflowRemote Members

        [OperationContract]
        public bool StartWorkflow(string MarkupFileName, string RulesFileName, WorkflowParameters Parameters)
        {
            ManagedWorkflowInstance instance = _manager.StartWorkflow(MarkupFileName, RulesFileName, new Dictionary<string, object>());
            return (instance != null);
        }

        #endregion
    }
}
