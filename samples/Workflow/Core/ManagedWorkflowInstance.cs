using System;
using System.Collections.Generic;
using System.Threading;
using System.Workflow.Runtime;

namespace CdcSoftware.Workflow.Core
{
	public class ManagedWorkflowInstance
	{
        private WorkflowInstance _instance;
        private ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private Dictionary<string, object> _outputs = new Dictionary<string, object>();
        private Exception _exception;
        private string _reasonSuspended = string.Empty;

        public ManagedWorkflowInstance(WorkflowInstance Instance)
        {
            _instance = Instance;
        }

        public Guid Id
        {
            get
            {
                if (_instance != null)
                {
                    return _instance.InstanceId;
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }
        public Dictionary<string, object> Outputs
        {
            get { return _outputs; }
            set { _outputs = value; }
        }
        public ManualResetEvent WaitHandle
        {
            get { return _waitHandle; }
        }
        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }
        public string ReasonSuspended
        {
            get { return _reasonSuspended; }
            set { _reasonSuspended = value; }
        }
        public WorkflowInstance WorkflowInstance
        {
            get { return _instance; }
        }

        public void StopWaiting()
        {
            _waitHandle.Set();
        }
	}
}
