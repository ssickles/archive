using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Xml;
using System.Threading;

namespace WickedSick.Workflow.Core
{
	public class WorkflowApplication: IDisposable
	{
        private string _name;
        private WorkflowRuntime _runtime;
        private ManualResetEvent _startHandle = new ManualResetEvent(false);
        private Dictionary<Guid, WorkflowInstance> _workflows = new Dictionary<Guid, WorkflowInstance>();

        public WorkflowApplication(string Name)
        {
            _name = Name;
            _runtime = new WorkflowRuntime("WorkflowRuntime");
            SubscribeToEvents();
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void StartRuntime()
        {
            _runtime.StartRuntime();
            _startHandle.WaitOne();
        }

        public void StartWorkflow(Type WorkflowType, Dictionary<string, object> Parameters)
        {
            WorkflowInstance instance = _runtime.CreateWorkflow(WorkflowType, Parameters);
            _workflows.Add(instance.InstanceId, instance);
            instance.Start();
        }

        public void StartWorkflow(string MarkupFileName, string RulesMarkupFileName, Dictionary<string, object> Parameters)
        {
            WorkflowInstance instance = null;
            XmlReader wfReader = null;
            XmlReader rulesReader = null;
            try
            {
                wfReader = XmlReader.Create(MarkupFileName);
                if (!string.IsNullOrEmpty(RulesMarkupFileName))
                {
                    rulesReader = XmlReader.Create(RulesMarkupFileName);
                    //create the workflow with workflow and rules
                    instance = _runtime.CreateWorkflow(
                        wfReader, rulesReader, Parameters);
                }
                else
                {
                    //create the workflow with workflow markup only
                    instance = _runtime.CreateWorkflow(
                        wfReader, null, Parameters);
                }

                _workflows.Add(instance.InstanceId, instance);
                instance.Start();
            }
            finally
            {
                if (wfReader != null)
                {
                    wfReader.Close();
                }
                if (rulesReader != null)
                {
                    rulesReader.Close();
                }
            }
        }

        public void StopRuntime()
        {
            _runtime.StopRuntime();
        }

        private void SubscribeToEvents()
        {
            _runtime.Started += new EventHandler<WorkflowRuntimeEventArgs>(_runtime_Started);
            _runtime.Stopped += new EventHandler<WorkflowRuntimeEventArgs>(_runtime_Stopped);
            _runtime.WorkflowAborted += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowAborted);
            _runtime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(_runtime_WorkflowCompleted);
            _runtime.WorkflowCreated += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowCreated);
            _runtime.WorkflowIdled += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowIdled);
            _runtime.WorkflowLoaded += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowLoaded);
            _runtime.WorkflowPersisted += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowPersisted);
            _runtime.WorkflowResumed += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowResumed);
            _runtime.WorkflowStarted += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowStarted);
            _runtime.WorkflowSuspended += new EventHandler<WorkflowSuspendedEventArgs>(_runtime_WorkflowSuspended);
            _runtime.WorkflowTerminated += new EventHandler<WorkflowTerminatedEventArgs>(_runtime_WorkflowTerminated);
            _runtime.WorkflowUnloaded += new EventHandler<WorkflowEventArgs>(_runtime_WorkflowUnloaded);
        }

        private void _runtime_WorkflowUnloaded(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowUnloaded");
        }

        private void _runtime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowTerminated");
            //ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            //if (managedInstance != null)
            //{
            //    managedInstance.Exception = e.Exception;
            //    managedInstance.StopWaiting();
            //}
        }

        private void _runtime_WorkflowSuspended(object sender, WorkflowSuspendedEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowSuspended");
            //ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            //if (managedInstance != null)
            //{
            //    managedInstance.ReasonSuspended = e.Error;
            //}
        }
        
        private void _runtime_WorkflowStarted(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowStarted");
        }

        private void _runtime_WorkflowResumed(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowResumed");
        }

        private void _runtime_WorkflowPersisted(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowPersisted");
        }

        private void _runtime_WorkflowLoaded(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowLoaded");
        }

        private void _runtime_WorkflowIdled(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowIdled");
        }

        private void _runtime_WorkflowCreated(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowCreated");
        }

        private void _runtime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowCompleted");
            //ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            //if (managedInstance != null)
            //{
            //    managedInstance.Outputs = e.OutputParameters;
            //    managedInstance.StopWaiting();
            //}
        }

        private void _runtime_WorkflowAborted(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowAborted");
            //ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            //if (managedInstance != null)
            //{
            //    managedInstance.StopWaiting();
            //}
        }

        private void _runtime_Stopped(object sender, WorkflowRuntimeEventArgs e)
        {
            LogStatus(Guid.Empty, "Stopped");

        }

        private void _runtime_Started(object sender, WorkflowRuntimeEventArgs e)
        {
            LogStatus(Guid.Empty, "Started");
            _startHandle.Set();
        }

        private void LogStatus(Guid Id, string Message)
        {
            string formattedMessage = string.Empty;
            if (Id == Guid.Empty)
            {
                formattedMessage = string.Format("Runtime = {0}", Message);
            }
            else
            {
                formattedMessage = string.Format("{0} - {1}", Id, Message);
            }
            Console.WriteLine(formattedMessage);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_runtime != null)
            {
                _runtime.StopRuntime();
                _runtime.Dispose();
            }
            _workflows.Clear();
        }

        #endregion
    }
}
