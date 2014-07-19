using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace WcfBehaviorExtensionSandbox
{
    public class BiometricValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            //var identityUid = new Guid(userName);
            //OperationContext
            //OperationContext.Current.IncomingMessageProperties.Add("IdentityUid", identityUid);
        }
    }
}
