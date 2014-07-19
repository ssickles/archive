using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Workflow.Runtime;
using System.Collections.Generic;
using WickedSick.Workflow.Core;
using System.Diagnostics;

namespace WickedSick.Services
{
    partial class WorkflowEngine : ServiceBase
    {
        private static EventLog _log;
        private Thread _main;
        private ManualResetEvent _resetHandle = new ManualResetEvent(false);
        private static Dictionary<string, WorkflowApplication> _applications;
        public ServiceHost _service;

        public WorkflowEngine()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (EventLog.SourceExists("WickedSick"))
            {
                EventLog.DeleteEventSource("WickedSick");
            }
            EventLog.CreateEventSource("WickedSick", "Workflow");

            _log = new EventLog();
            _log.Log = "Workflow";
            _log.Source = "WickedSick";

            _log.WriteEntry("Log Created and Setup.");

            _applications = new Dictionary<string, WorkflowApplication>();
            _main = new Thread(new ThreadStart(Run));
            _main.Start();

            _log.WriteEntry("Main Thread Started.");

            if (_service != null)
            {
                _log.WriteEntry("ServiceHost already exists.");
                _service.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            WorkflowHost host = new WorkflowHost();
            host.StartWorkflowEvent += new WorkflowHost.StartWorkflowHandler(host_StartWorkflowEvent);
            _service = new ServiceHost(host);
            _log.WriteEntry("ServiceHost created.");

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            _service.Open();
            _log.WriteEntry("ServiceHost Open.");

            _log.WriteEntry("Workflow Service Started.");
        }

        void host_StartWorkflowEvent(object sender, StartWorkflowEventArgs StartWorkflowArgs)
        {
            _log.WriteEntry(string.Format("StartWorkflow Event called.{0}Application: {1}, Workflow: {2}, Parameters: {3}",
                Environment.NewLine, StartWorkflowArgs.ApplicationName, StartWorkflowArgs.WorkflowName, StartWorkflowArgs.Parameters.Count));
        }

        protected override void OnStop()
        {
            if (_service != null)
            {
                _log.WriteEntry("Closing ServiceHost.");
                _service.Close();
                _service = null;
            }

            _resetHandle.Set();

            _log.WriteEntry("Workflow Service Stopped.");
        }

        private void Run()
        {
            WorkflowApplication app1 = new WorkflowApplication("app1");
            _applications.Add("app1", app1);
            app1.StartRuntime();

            _log.WriteEntry("app1 Started.");

            WorkflowApplication app2 = new WorkflowApplication("app2");
            _applications.Add("app2", app2);
            app2.StartRuntime();

            _log.WriteEntry("app2 Started.");

            _resetHandle.WaitOne();
        }
    }
}
