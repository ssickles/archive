using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WickedSick.Workflow.Core.Test
{
	public class WorkflowHostTest
	{
        public static void Run()
        {
            WorkflowHost host = new WorkflowHost();
            ServiceHost service = new ServiceHost(host);
            service.Open();

            Console.ReadLine();
        }
	}
}
