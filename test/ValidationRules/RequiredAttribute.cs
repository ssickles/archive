using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationRules
{
    public class RequiredAttribute: ValidationBase
    {
        public override string Validate(string Name, object Value)
        {
            if (Value == null)
                throw new ApplicationException(string.Format("{0} is required.", Name));
            else
                return string.Empty;
        }
    }
}
