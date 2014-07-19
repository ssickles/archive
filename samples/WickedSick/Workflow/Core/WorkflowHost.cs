using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WickedSick.Workflow.Core
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class WorkflowHost: IWorkflowHost
	{
        public delegate void StartWorkflowHandler(object sender, StartWorkflowEventArgs StartWorkflowArgs);
        public event StartWorkflowHandler StartWorkflowEvent;

        #region IWorkflowHost Members

        public void StartWorkflow(string ApplicationName, string WorkflowName, Dictionary<string, object> Parameters)
        {
            OnStartWorkflow(ApplicationName, WorkflowName, Parameters);
        }

        #endregion

        private void OnStartWorkflow(string ApplicationName, string WorkflowName, Dictionary<string, object> Parameters)
        {
            StartWorkflowHandler _eventObj = StartWorkflowEvent;
            if (_eventObj != null)
            {
                _eventObj(this, new StartWorkflowEventArgs(ApplicationName, WorkflowName, Parameters));
            }
        }
    }

    public class StartWorkflowEventArgs: EventArgs
    {
        private string _applicationName;
        private string _workflowName;
        private Dictionary<string, object> _parameters;

        public StartWorkflowEventArgs(string ApplicationName, string WorkflowName, Dictionary<string, object> Parameters): base()
        {
            _applicationName = ApplicationName;
            _workflowName = WorkflowName;
            _parameters = Parameters;
        }

        public string ApplicationName
        {
            get { return _applicationName; }
        }

        public string WorkflowName
        {
            get { return _workflowName; }
        }

        public Dictionary<string, object> Parameters
        {
            get { return _parameters; }
        }
    }
}
