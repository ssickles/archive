using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Workflow.Runtime;
using CdcSoftware.Workflow.Core;

namespace CdcSoftware.Workflow.Services
{
    partial class WorkflowEngine : ServiceBase
    {
        WorkflowRuntimeManager _manager;
        ServiceHost _remote;

        private ManualResetEvent _resetHandle = new ManualResetEvent(false);

        public WorkflowEngine()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread runtime = new Thread(new ThreadStart(Run));
            runtime.Start();

            WorkflowRemote singleton = new WorkflowRemote(_manager);
            _remote = new ServiceHost(singleton);
            _remote.Open();
       }

        protected override void OnStop()
        {
            _resetHandle.Set();
            if (_remote != null)
            {
                _remote.Close();
            }
        }

        private void Run()
        {
            using (_manager = new WorkflowRuntimeManager(new WorkflowRuntime("WorkflowRuntime")))
            {
                _manager.MessageEvent += new EventHandler<WorkflowLogEventArgs>(manager_MessageEvent);
                _manager.WorkflowRuntime.StartRuntime();

                _resetHandle.WaitOne();
            }
        }

        void manager_MessageEvent(object sender, WorkflowLogEventArgs e)
        {
        }
    }
}
