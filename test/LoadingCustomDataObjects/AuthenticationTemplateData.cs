using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingCustomDataObjects
{
    [TableAttribute("authentication_templates")]
    public class AuthenticationTemplateData
    {
        [FieldAttribute("id")]
        public int Id { get; set; }
        [FieldAttribute("authentication_unit_code")]
        public string AuthenticationUnitCode { get; set; }
        [FieldAttribute("template")]
        public byte[] Template { get; set; }
        [FieldAttribute("enrollment_id")]
        public int EnrollmentId { get; set; }
        [FieldAttribute("score")]
        public int Score { get; set; }
    }
}
