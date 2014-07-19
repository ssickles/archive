using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace CdcSoftware.Workflow.Core.Interfaces
{
    [ServiceContract]
	public interface IWorkflowRemote
	{
        [OperationContract]
        bool StartWorkflow(string MarkupFileName, string RulesFileName, WorkflowParameters Parameters);
	}
}
