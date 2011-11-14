using System;
using System.ServiceModel.Configuration;

namespace WcfBehaviorExtensionSandbox
{
    public class FlatWsdlExportEndpointBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(FlatWsdlExportEndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new FlatWsdlExportEndpointBehavior();
        }
    }
}
