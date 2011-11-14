using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;

namespace ConsoleApplication10
{
    public class SomeValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            return true;
        }
    }
}
