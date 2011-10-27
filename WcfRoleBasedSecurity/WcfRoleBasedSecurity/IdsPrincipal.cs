using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace WcfRoleBasedSecurity
{
    public class IdsPrincipal: IPrincipal
    {
        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            return Identity.IsAuthenticated;
        }
    }
}
