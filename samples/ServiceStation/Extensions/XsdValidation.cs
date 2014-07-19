using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Xml.Schema;
using System.Xml;

namespace Extensions
{
    public class XsdValidation : Attribute, IServiceBehavior, IEndpointBehavior
    {

        #region IServiceBehavior Members

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
            WsdlExporter wsdlExporter = new WsdlExporter();
            wsdlExporter.ExportEndpoints(serviceDescription.Endpoints,
                new XmlQualifiedName(serviceDescription.Name, serviceDescription.Namespace));

            foreach (ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
                foreach (EndpointDispatcher endpointDispatcher in cDispatcher.Endpoints)
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                        new XsdValidationInspector(wsdlExporter.GeneratedXmlSchemas));
        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
        }

        #endregion

        #region IEndpointBehavior Members

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            WsdlExporter wsdlExporter = new WsdlExporter();
            wsdlExporter.ExportEndpoint(endpoint);
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
                new XsdValidationInspector(wsdlExporter.GeneratedXmlSchemas));
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
