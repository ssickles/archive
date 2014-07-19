using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WcfBehaviorExtensionSandbox
{
    public class ApplicationRequestContextAttribute : Attribute, IServiceBehavior, IDispatchMessageInspector
    {
        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            var dispatchRuntimes = serviceHostBase.ChannelDispatchers
                .Cast<ChannelDispatcher>()
                .SelectMany(cd => cd.Endpoints)
                .Select(ed => ed.DispatchRuntime);

            foreach (var dispatchRuntime in dispatchRuntimes)
            {
                dispatchRuntime.MessageInspectors.Add(this);
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

        #endregion

        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            OperationContext.Current.Extensions.Add(new ApplicationRequestContext());
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            OperationContext.Current.Extensions.Remove(ApplicationRequestContext.Current);
        }

        #endregion
    }
}
