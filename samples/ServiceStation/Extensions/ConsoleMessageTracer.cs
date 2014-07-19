using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;

namespace Extensions
{
    public class ConsoleMessageTracer : IDispatchMessageInspector, IClientMessageInspector
    {
        private Message TraceMessage(MessageBuffer buffer)
        {
            Message msg = buffer.CreateMessage();
            Console.WriteLine("\n{0}\n", msg);
            return buffer.CreateMessage();
        }

        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, 
            InstanceContext instanceContext)
        {
            request = TraceMessage(request.CreateBufferedCopy(int.MaxValue));
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            reply = TraceMessage(reply.CreateBufferedCopy(int.MaxValue));
        }

        #endregion

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            reply = TraceMessage(reply.CreateBufferedCopy(int.MaxValue));
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request = TraceMessage(request.CreateBufferedCopy(int.MaxValue));
            return null;
        }

        #endregion
    }
}
