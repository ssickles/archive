using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CdcSoftware.Workflow.Core.Interfaces;

namespace CdcSoftware.Workflow.Core.Test
{
	public class WorkflowRemoteTest
	{
        public static void Run()
        {
            Test.WorkflowRemoteClient remote = new CdcSoftware.Workflow.Core.Test.Test.WorkflowRemoteClient();
            remote.StartWorkflow(@"C:\Documents and Settings\Administrator\Desktop\Pro WF\chapter 17\WorkflowDesignerApp\bin\Debug\Test.xoml", @"C:\Documents and Settings\Administrator\Desktop\Pro WF\chapter 17\WorkflowDesignerApp\bin\Debug\Test.rules", new WorkflowParameters());
        }
	}
}
