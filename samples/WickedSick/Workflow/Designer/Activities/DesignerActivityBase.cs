using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;
using System.Windows.Controls;

namespace WickedSick.Workflow.DesignerActivities
{
    public class DesignerActivityBase: UserControl
    {
        private string _activityName;

        public DesignerActivityBase()
        {

        }

        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }
    }
}
