using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Net;
using System.ServiceModel;

namespace ConsoleClient
{
    public class CustomHeaderInjector : IClientMessageInspector
    {
        private string USER_AGENT_HTTP_HEADER = "userId";
        private string m_userAgent = "1";
        
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            return;
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;
            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
            }
            else
            {
                httpRequestMessage = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            }
            var header = httpRequestMessage.Headers[USER_AGENT_HTTP_HEADER];
            if (header == null)
            {
                httpRequestMessage.Headers.Add(USER_AGENT_HTTP_HEADER, this.m_userAgent);
            }
            else
            {
                httpRequestMessage.Headers[USER_AGENT_HTTP_HEADER] = this.m_userAgent;
            }
            return null;
        }
    }
}
