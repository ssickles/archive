using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace WcfRoleBasedSecurity
{
    public class IdsIdentity: IIdentity
    {
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }
    }
}
