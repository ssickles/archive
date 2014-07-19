using System;

namespace IdentityStream.Models
{
    [Serializable]
    public class SecureContextCredentials
    {
        public Guid IdentityUid { get; set; }
        public string ApplicationCode { get; set; }
        public string TransactionCode { get; set; }
    }
}