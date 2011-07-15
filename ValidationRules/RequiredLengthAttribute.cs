using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Reflection;

namespace ValidationRules
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredLengthAttribute : ValidationBase
    {
        public RequiredLengthAttribute(int Min, int Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        public int Min { get; private set; }
        public int Max { get; private set; }

        public override string Validate(string Name, object Value)
        {
            string pv = Value as string;
            if (pv == null || (pv.Length < Min || pv.Length > Max))
                return string.Format("{0} length must be between {1} and {2}.", Name, Min, Max);
            else
                return string.Empty;
        }
    }
}
