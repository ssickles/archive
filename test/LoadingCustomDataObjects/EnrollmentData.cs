using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadingCustomDataObjects
{
    [TableAttribute("enrollments")]
    public class EnrollmentData
    {
        [FieldAttribute("id")]
        public int Id { get; set; }
        [FieldAttribute("identity_id")]
        public int IdentityId { get; set; }
        [FieldAttribute("administrator_id")]
        public int AdministratorId { get; set; }
        [FieldAttribute("enrollment_date")]
        public DateTime EnrollmentDate { get; set; }
        [FieldAttribute("active")]
        public bool Active { get; set; }
    }
}
