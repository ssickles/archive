using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WickedSick.Workflow.Core.Test
{
	public class WorkflowApplicationTest
	{
        private static Dictionary<string, WorkflowApplication> _applications = new Dictionary<string, WorkflowApplication>();

        public static void Run()
        {
            WorkflowApplication app1 = new WorkflowApplication("app1");
            _applications.Add("app1", app1);
            app1.StartRuntime();

            Dictionary<string, object> wfArguments = new Dictionary<string, object>();
            wfArguments.Add("InputString", "1");
            _applications["app1"].StartWorkflow(typeof(Workflow1), wfArguments);
            _applications["app1"].StartWorkflow(typeof(Workflow1), wfArguments);
            _applications["app1"].StartWorkflow(typeof(Workflow1), wfArguments);

            WorkflowApplication app2 = new WorkflowApplication("app2");
            _applications.Add("app2", app2);
            app2.StartRuntime();

            _applications["app1"].StartWorkflow(typeof(Workflow1), wfArguments);
            _applications["app1"].StartWorkflow(typeof(Workflow1), wfArguments);

            Thread.Sleep(15000);

            app1.Dispose();
            app2.Dispose();

            Console.ReadLine();
        }
	}
}
