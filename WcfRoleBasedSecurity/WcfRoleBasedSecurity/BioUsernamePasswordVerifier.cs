using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Threading;
using System.IdentityModel.Tokens;

namespace WcfRoleBasedSecurity
{
    class BioUsernamePasswordVerifier : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName == "scott" && password == "sas0927")
            {
                Thread.CurrentPrincipal =
                    new IdsPrincipal()
                    {
                        Identity =
                        new IdsIdentity()
                        {
                            IsAuthenticated = true,
                            Name = "Scott Sickles"
                        }
                    };
            }
            else
            {
                throw new SecurityTokenException("Unknown Username or Password");
            }
        }
    }
}
