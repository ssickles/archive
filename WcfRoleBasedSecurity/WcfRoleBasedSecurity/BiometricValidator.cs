using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Threading;
using System.IdentityModel.Tokens;

namespace IdentityStream.Services.ApplicationService
{
    class BiometricValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {

        }
    }
}
