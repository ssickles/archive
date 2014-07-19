using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace WickedSick.Workflow.Core.Test
{
	public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public static DependencyProperty InputStringProperty = DependencyProperty.Register("InputString", typeof(string), typeof(Workflow1));
        public static DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(string), typeof(Workflow1));

        [DescriptionAttribute("Result")]
        [CategoryAttribute("Result Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Result
        {
            get
            {
                return ((string)(base.GetValue(Workflow1.ResultProperty)));
            }
            set
            {
                base.SetValue(Workflow1.ResultProperty, value);
            }
        }
        [DescriptionAttribute("InputString")]
        [CategoryAttribute("InputString Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string InputString
        {
            get
            {
                return ((string)(base.GetValue(Workflow1.InputStringProperty)));
            }
            set
            {
                base.SetValue(Workflow1.InputStringProperty, value);
            }
        }

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            Console.WriteLine("{0} executing, parameter={1}", this.WorkflowInstanceId, InputString);
            Result = string.Format("{0} result property", this.WorkflowInstanceId);
        }
    }
}
