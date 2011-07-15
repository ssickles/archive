using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingCustomDataObjects
{
    [TableAttribute("authentication_units")]
    public class AuthenticationUnitData
    {
        [FieldAttribute("id", true)]
        public int Id { get; set; }
        [FieldAttribute("code")]
        public string Code { get; set; }
        [FieldAttribute("authentication_type_code")]
        public string AuthenticationType { get; set; }
        [FieldAttribute("description")]
        public string Description { get; set; }
        [FieldAttribute("sequence")]
        public int EnrollmentSequence { get; set; }
    }
}
