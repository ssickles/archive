using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SecuredWcfService
{
    [DataContract]
    [KnownType(typeof(DatabaseOfflineErrorDetails))]
    [KnownType(typeof(DatabaseProcessingErrorDetails))]
    [KnownType(typeof(ApplicationErrorDetails))]
    public class ErrorDetails
    {
        public ErrorDetails(string MethodName, string DateTime)
        {
            this.MethodName = MethodName;
            this.DateTime = DateTime;
        }

        [DataMember]
        public string MethodName;

        [DataMember]
        public string DateTime;
    }

    [DataContract]
    public class DatabaseOfflineErrorDetails: ErrorDetails 
    {
        public DatabaseOfflineErrorDetails(string MethodName, string DateTime) : base(MethodName, DateTime) { }
    }

    [DataContract]
    public class DatabaseProcessingErrorDetails : ErrorDetails 
    {
        public DatabaseProcessingErrorDetails(string MethodName, string DateTime) : base(MethodName, DateTime) { }
    }

    [DataContract]
    public class ApplicationErrorDetails : ErrorDetails 
    {
        public ApplicationErrorDetails(string MethodName, string DateTime) : base(MethodName, DateTime) { }
    }
}
