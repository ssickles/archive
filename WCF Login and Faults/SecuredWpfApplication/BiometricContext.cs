using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecuredWpfApplication.SecuredService;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SecuredWpfApplication
{
    public class BiometricContext
    {
        private static ISecuredService _proxy;

        static BiometricContext()
        {
            _proxy = new SecuredServiceClient();
            SecuredServiceClient client = new SecuredServiceClient();
            client.ChannelFactory.Credentials.UserName.UserName = "scottsi";
            client.ChannelFactory.Credentials.UserName.Password = "pa$$w0rd";
            //client.Endpoint.Behaviors.Add(new LoginBehavior());
            //EndpointAddress address = new EndpointAddress("http://localhost:8000/SecuredService");
            //WSHttpBinding binding = new WSHttpBinding();
            //ChannelFactory<ISecuredService> cf = new ChannelFactory<ISecuredService>(binding, address);            
        }

        public static ISecuredService Current
        {
            get
            {
                return _proxy;
            }
        }
    }

    public class LoginBehavior : IEndpointBehavior
    {
        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new LoginMessageInspector());
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class LoginMessageInspector : IClientMessageInspector
    {
        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            throw new NotImplementedException();
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
