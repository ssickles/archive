using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace LoadingCustomDataObjects
{
    [TableAttribute("authentication_levels")]
    public class AuthenticationLevelData
    {
        [FieldAttribute("id", true)]
        public int Id { get; set; }
        [FieldAttribute("description")]
        public string Description { get; set; }
    }
}
