using System;
using System.Collections.Generic;
using System.Threading;
using System.Workflow.Runtime;
using System.Xml;

namespace CdcSoftware.Workflow.Core
{
	public class WorkflowRuntimeManager: IDisposable
	{
        private WorkflowRuntime _runtime;
        private Dictionary<Guid, ManagedWorkflowInstance> _workflows = new Dictionary<Guid, ManagedWorkflowInstance>();

        public WorkflowRuntimeManager(WorkflowRuntime Runtime)
        {
            _runtime = Runtime;
            if (_runtime == null)
            {
                throw new NullReferenceException("WorkflowRuntime can not be null.");
            }

            SubscribeToEvents();
        }

        public WorkflowRuntime WorkflowRuntime
        {
            get { return _runtime; }
        }
        public Dictionary<Guid, ManagedWorkflowInstance> Workflows
        {
            get { return _workflows; }
        }
        public event EventHandler<WorkflowLogEventArgs> MessageEvent;

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

        public ManagedWorkflowInstance StartWorkflow(Type WorkflowType,
            Dictionary<string, object> Parameters)
        {
            WorkflowInstance instance = _runtime.CreateWorkflow(WorkflowType, Parameters);
            ManagedWorkflowInstance managedInstance = TrackWorkflow(instance);
            instance.Start();
            return managedInstance;
        }
        public ManagedWorkflowInstance StartWorkflow(string MarkupFileName,
            string RulesMarkupFileName,
            Dictionary<string, object> Parameters)
        {
            WorkflowInstance instance = null;
            ManagedWorkflowInstance managedInstance = null;
            XmlReader wfReader = null;
            XmlReader rulesReader = null;
            try
            {
                wfReader = XmlReader.Create(MarkupFileName);
                if (!String.IsNullOrEmpty(RulesMarkupFileName))
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

                managedInstance = TrackWorkflow(instance);
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
            return managedInstance;
        }
        public void WaitAll(int MSecondsWait)
        {
            if (_workflows.Count > 0)
            {
                WaitHandle[] handles = new WaitHandle[_workflows.Count];
                int i = 0;
                foreach (ManagedWorkflowInstance managedInstance in _workflows.Values)
                {
                    handles[i] = managedInstance.WaitHandle;
                    i++;
                }
                WaitHandle.WaitAll(handles, MSecondsWait, false);
            }
        }

        private ManagedWorkflowInstance TrackWorkflow(WorkflowInstance instance)
        {
            ManagedWorkflowInstance managedInstance = null;
            if (!_workflows.ContainsKey(instance.InstanceId))
            {
                managedInstance = new ManagedWorkflowInstance(instance);
                _workflows.Add(managedInstance.Id, managedInstance);
            }
            return managedInstance;
        }
        public void ClearWorkflow(Guid Id)
        {
            if (_workflows.ContainsKey(Id))
            {
                _workflows.Remove(Id);
            }

        }
        public void ClearAllWorkflows()
        {
            _workflows.Clear();
        }
        public ManagedWorkflowInstance FindWorkflow(Guid Id)
        {
            if (_workflows.ContainsKey(Id))
            {
                return _workflows[Id];
            }
            else
            {
                return null;
            }
        }

        void _runtime_WorkflowUnloaded(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowUnloaded");
        }
        void _runtime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowTerminated");
            ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            if (managedInstance != null)
            {
                managedInstance.Exception = e.Exception;
                managedInstance.StopWaiting();
            }
        }
        void _runtime_WorkflowSuspended(object sender, WorkflowSuspendedEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowSuspended");
            ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            if (managedInstance != null)
            {
                managedInstance.ReasonSuspended = e.Error;
            }
        }
        void _runtime_WorkflowStarted(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowStarted");
        }
        void _runtime_WorkflowResumed(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowResumed");
        }
        void _runtime_WorkflowPersisted(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowPersisted");
        }
        void _runtime_WorkflowLoaded(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowLoaded");
        }
        void _runtime_WorkflowIdled(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowIdled");
        }
        void _runtime_WorkflowCreated(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowCreated");
        }
        void _runtime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowCompleted");
            ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            if (managedInstance != null)
            {
                managedInstance.Outputs = e.OutputParameters;
                managedInstance.StopWaiting();
            }
        }
        void _runtime_WorkflowAborted(object sender, WorkflowEventArgs e)
        {
            LogStatus(e.WorkflowInstance.InstanceId, "WorkflowAborted");
            ManagedWorkflowInstance managedInstance = FindWorkflow(e.WorkflowInstance.InstanceId);
            if (managedInstance != null)
            {
                managedInstance.StopWaiting();
            }
        }
        void _runtime_Stopped(object sender, WorkflowRuntimeEventArgs e)
        {
            LogStatus(Guid.Empty, "Stopped");
        }
        void _runtime_Started(object sender, WorkflowRuntimeEventArgs e)
        {
            LogStatus(Guid.Empty, "Started");
        }

        private void LogStatus(Guid Id, string Message)
        {
            if (MessageEvent != null)
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
                MessageEvent(this, new WorkflowLogEventArgs(formattedMessage));
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (_runtime != null)
            {
                _runtime.StopRuntime();
                _runtime.Dispose();
            }
            ClearAllWorkflows();
        }
        #endregion
    }
}
