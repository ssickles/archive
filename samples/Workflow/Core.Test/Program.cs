using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;

namespace CdcSoftware.Workflow.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowRemoteTest.Run();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
