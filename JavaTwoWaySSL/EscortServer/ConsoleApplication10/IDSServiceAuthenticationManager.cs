using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IdentityModel.Policy;
using System.Collections.ObjectModel;

namespace ConsoleApplication10
{
    public class IDSServiceAuthenticationManager : ServiceAuthenticationManager
    {
        public override ReadOnlyCollection<IAuthorizationPolicy> Authenticate(ReadOnlyCollection<IAuthorizationPolicy> authPolicy, Uri listenUri, ref Message message)
        {
            return base.Authenticate(authPolicy, listenUri, ref message);
        }
    }
}
