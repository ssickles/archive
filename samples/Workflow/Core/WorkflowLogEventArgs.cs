using System;

namespace CdcSoftware.Workflow.Core
{
    public class WorkflowLogEventArgs : EventArgs
    {
        private string _message = string.Empty;
        public WorkflowLogEventArgs(string Message)
        {
            _message = Message;
        }

        public string Message
        {
            get { return _message; }
        }
    }
}
