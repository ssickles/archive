using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingCustomDataObjects
{
    [TableAttribute("identities")]
    public class IdentityData
    {
        [FieldAttribute("id")]
        public int Id { get; set; }
        [FieldAttribute("first_name")]
        public string FirstName { get; set; }
        [FieldAttribute("last_name")]
        public string LastName { get; set; }
        [FieldAttribute("country_code")]
        public string CountryCode { get; set; }
        [FieldAttribute("active")]
        public bool Active { get; set; }
        [FieldAttribute("bio_enabled")]
        public bool BioEnabled { get; set; }
        [FieldAttribute("identity_code")]
        public string IdentityCode { get; set; }
        [FieldAttribute("t24_id")]
        public string T24Id { get; set; }
        [FieldAttribute("group_id")]
        public int GroupId { get; set; }
        [FieldAttribute("authentication_level_id")]
        public int AuthenticationLevelId { get; set; }
    }
}
