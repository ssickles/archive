using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Reflection;

namespace ValidationRules
{
    interface IValidation
    {
        string Validate(string Name, object Value);
    }
}
