using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace Extensions
{
    public class ConsoleMessageTracingElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(ConsoleMessageTracing); }
        }
        protected override object CreateBehavior()
        {
            return new ConsoleMessageTracing();
        }
    }
}
