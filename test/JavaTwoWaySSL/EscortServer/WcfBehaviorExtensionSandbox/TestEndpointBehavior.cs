using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;

namespace WcfBehaviorExtensionSandbox
{
    public class TestEndpointBehavior : IEndpointBehavior
    {
        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ContractFilter = new TestMessageFilter();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
