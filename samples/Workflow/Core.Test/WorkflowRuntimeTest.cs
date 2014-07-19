using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using CdcSoftware.Workflow.Core;

namespace CdcSoftware.Workflow.Core.Test
{
	public class WorkflowRuntimeTest
	{
        public static void Run()
        {
            Console.WriteLine("Running test configured with App.config.");

            using (WorkflowRuntimeManager manager = new WorkflowRuntimeManager(new WorkflowRuntime("WorkflowRuntime")))
            {
                manager.MessageEvent += delegate(object sender, WorkflowLogEventArgs e)
                {
                    Console.WriteLine(e.Message);
                };
                
                manager.WorkflowRuntime.StartRuntime();

                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                //wfArguments.Add("InputString", "1");
                ManagedWorkflowInstance instance1 = manager.StartWorkflow(@"C:\Documents and Settings\Administrator\Desktop\Pro WF\chapter 17\WorkflowDesignerApp\bin\Debug\Test.xoml", @"C:\Documents and Settings\Administrator\Desktop\Pro WF\chapter 17\WorkflowDesignerApp\bin\Debug\Test.rules", wfArguments);
                //ManagedWorkflowInstance instance1 = manager.StartWorkflow(typeof(Workflow1), wfArguments);
                //instance1.WorkflowInstance.Terminate("Manual Termination");

                wfArguments.Clear();
                wfArguments.Add("InputString", "2");
                ManagedWorkflowInstance instance2 = manager.StartWorkflow(typeof(Workflow1), wfArguments);
                instance2.WorkflowInstance.Suspend("Manual Suspension");
                instance2.WorkflowInstance.Resume();

                wfArguments.Clear();
                wfArguments.Add("InputString", "3");
                manager.StartWorkflow(typeof(Workflow1), wfArguments);
                
                manager.WaitAll(15000);

                foreach (ManagedWorkflowInstance managedInstance in manager.Workflows.Values)
                {
                    if (managedInstance.Outputs.ContainsKey("Result"))
                    {
                        Console.WriteLine(managedInstance.Outputs["Result"]);
                    }
                    if (managedInstance.Exception != null)
                    {
                        Console.WriteLine("{0} - Exception: {1}", managedInstance.Id, managedInstance.Exception.Message);
                    }
                    if (managedInstance.ReasonSuspended.Length > 0)
                    {
                        Console.WriteLine("{0} - Suspended: {1}", managedInstance.Id, managedInstance.ReasonSuspended);
                    }
                }
                manager.ClearAllWorkflows();
            }
        }
	}
}
