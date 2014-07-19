using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace WcfBehaviorExtensionSandbox
{
    public class TestMessageFilter : MessageFilter
    {
        public override bool Match(System.ServiceModel.Channels.Message message)
        {
            return true;
        }

        public override bool Match(System.ServiceModel.Channels.MessageBuffer buffer)
        {
            return true;

        }
    }
}
