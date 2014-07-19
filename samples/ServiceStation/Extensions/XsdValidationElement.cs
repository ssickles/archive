using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Configuration;

namespace Extensions
{
    public class XsdValidationElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(XsdValidation);  }
        }
        protected override object CreateBehavior()
        {
            return new XsdValidation();
        }
    }
}
