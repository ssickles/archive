using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WickedSick.Workflow.DesignerActivities
{
    public class ActivityBase : UserControl
    {
        private string _activityName;

        public ActivityBase()
        {

        }

        public ActivityBase(string ActivityName)
        {
            _activityName = ActivityName;
        }

        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }
    }
}
