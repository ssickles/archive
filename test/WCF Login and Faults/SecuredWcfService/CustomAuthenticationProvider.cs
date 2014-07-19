using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.ServiceModel.Security;

namespace SecuredWcfService
{
    public class CustomAuthenticationProvider: UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (!userName.ToLower().Equals("scott") || !password.Equals("sas0927"))
            {
                throw new SecurityNegotiationException("Invalid username or password.");
            }
        }
    }
}
