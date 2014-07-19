using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Extensions
{
    public class NoBasicEndpointValidator : Attribute, IServiceBehavior
    {
        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ServiceEndpoint se in serviceDescription.Endpoints)
                if (se.Binding.Name.Equals("BasicHttpBinding"))
                    throw new FaultException("BasicHttpBinding is not allowed. Please choose a secure binding.");
        }

        #endregion
    }
}
