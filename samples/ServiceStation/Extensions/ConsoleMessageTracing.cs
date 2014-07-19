using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace Extensions
{
    public class ConsoleMessageTracing : Attribute, IEndpointBehavior, IServiceBehavior
    {
        #region IEndpointBehavior Members

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new ConsoleMessageTracer());
        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new ConsoleMessageTracer());
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion

        #region IServiceBehavior Members

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
                foreach (EndpointDispatcher endpointDispatcher in cDispatcher.Endpoints)
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                        new ConsoleMessageTracer());
        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
