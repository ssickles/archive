using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace WcfRoleBasedSecurity
{
    public enum AuthenticationType
    {
        Bioemtrics,
        Certificate
    }

    public class IdsIdentity: IIdentity
    {
        public IdsIdentity(bool isAuthenticated, AuthenticationType authenticationType, Guid identityUid)
        {
            IsAuthenticated = isAuthenticated;
            _authenticationType = authenticationType;
            IdentityUid = identityUid;
        }

        public IdsIdentity(bool isAuthenticated, AuthenticationType authenticationType, string userId)
        {
            IsAuthenticated = isAuthenticated;
            _authenticationType = authenticationType;
            UserId = userId;
        }

        private AuthenticationType _authenticationType;
        public string AuthenticationType
        {
            get
            {
                return AuthenticationType.ToString();
            }
        }

        public bool IsAuthenticated { get; private set; }

        public string UserId { get; private set; }
        public Guid IdentityUid { get; private set; }

        public string Name
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
