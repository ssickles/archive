using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace WcfRoleBasedSecurity
{
    public class IdsPrincipal: IPrincipal
    {
        public IdsPrincipal(IIdentity identity, IList<string> roles)
        {
            Identity = identity;
            Roles = roles;
        }

        public IIdentity Identity { get; private set; }
        private IList<string> Roles { get; set; }

        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }
    }
}
