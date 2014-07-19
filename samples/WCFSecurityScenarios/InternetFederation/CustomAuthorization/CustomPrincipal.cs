// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.IdentityModel.Claims;

namespace CustomAuthorization
{
    public class CustomIdentity : IIdentity
    {

        private string m_name;

        public CustomIdentity(string name)
        {
            this.m_name = name;
        }

        #region IIdentity Members

        string IIdentity.AuthenticationType
        {
            get {
                return "CustomAuthorizationPolicy";
                }
        }

        bool IIdentity.IsAuthenticated
        {
            get 
            {
                return true;
            }
        }

        string IIdentity.Name
        {
            get
            {
                return m_name;
            }
        }

        #endregion
    }
    public interface IClaimSetPrincipal
    {
        ClaimSet Claims { get;}
        ClaimSet Issuer {get;}

        bool HasRequiredClaims(ClaimSet requiredClaims);
    }

    public class CustomPrincipal : IClaimSetPrincipal, IPrincipal 
    {
        private ClaimSet m_claims;
        private IIdentity m_identity;

        public CustomPrincipal(IIdentity identity, ClaimSet claims)
        {
            this.m_identity = identity;
            this.m_claims = claims;
        }

        #region IClaimSetPrincipal Members

        ClaimSet IClaimSetPrincipal.Issuer
        {
            get { return this.m_claims.Issuer; }
        }

        bool IClaimSetPrincipal.HasRequiredClaims(ClaimSet requiredClaims)
        {
            // if issuer is null, just check claims from anyone
            // if not, check issuer of claims as well

            return true;
        }

        ClaimSet IClaimSetPrincipal.Claims
        {
            get { return this.m_claims; }
        }

        #endregion

        #region IPrincipal Members

        IIdentity IPrincipal.Identity
        {
            get 
            {
                return this.m_identity;
            }
        }

        bool IPrincipal.IsInRole(string role)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
