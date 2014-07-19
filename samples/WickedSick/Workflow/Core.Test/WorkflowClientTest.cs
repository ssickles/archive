using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WickedSick.Workflow.Core.Test
{
    public class WorkflowClientTest
    {
        public static void Run()
        {
            WorkflowHostClient client = new WorkflowHostClient();
            string input = string.Empty;

            while (!input.Equals("s"))
            {
                Console.WriteLine("Starting Workflow: app1, Test.xoml, null");
                client.StartWorkflow("app1", "Test.xoml", new Dictionary<string, object>());

                input = Console.ReadLine();
            }
        }
    }
}
