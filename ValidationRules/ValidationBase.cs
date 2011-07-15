using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidationRules
{
    public abstract class ValidationBase: Attribute, IValidation
    {
        public abstract string Validate(string Name, object Value);
    }
}
