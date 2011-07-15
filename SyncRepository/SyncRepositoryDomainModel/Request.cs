using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SyncRepositoryDomainModel
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public DateTime LastRequest { get; set; }
    }
}
