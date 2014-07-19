// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
   
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Policy;
using System.IdentityModel.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Security;
using CustomAuthorization;

namespace Host
{
    public static class CustomClaimTypes
    {
        public const string Create = "http://schemas.thatindigogirl.com/samples/2006/07/identity/right/create";
        public const string Read = "http://schemas.thatindigogirl.com/samples/2006/07/identity/right/read";
        public const string Update = "http://schemas.thatindigogirl.com/samples/2006/07/identity/right/update";
        public const string Delete = "http://schemas.thatindigogirl.com/samples/2006/07/identity/right/delete";
    }

    public class HostAuthorizationPolicy : IAuthorizationPolicy
    {
        private Guid m_id;
        private ClaimSet m_issuer;

        public HostAuthorizationPolicy()
        {
            m_id = Guid.NewGuid();

            Claim c = Claim.CreateNameClaim("http://www.thatindigogirl.com/samples/2006/07/issuer");
            Claim[] claims = new Claim[1];
            claims[0] = c;
            m_issuer = new DefaultClaimSet(claims);
        }

        #region IAuthorizationPolicy Members

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
                
                // get claims from authorized issuer
                ClaimSet issuedClaims = null;
                foreach (ClaimSet cs in evaluationContext.ClaimSets)
                {
                    // If the issuer of the ClaimSet is this STS...
                    if ( cs.Issuer.ContainsClaim ( Claim.CreateDnsClaim("IPKey")))
                    {
                        issuedClaims = cs;
                    }
                }

                if (issuedClaims == null)
                {
                    throw new SecurityException("Unable to authenticate caller. Invalid claimset provided.");
                }

                CustomIdentity identity = new CustomIdentity("Claims");
                CustomPrincipal newPrincipal = new CustomPrincipal(identity, issuedClaims);

                evaluationContext.Properties["Principal"] = newPrincipal;

            return true;

        }

        private ClaimSet MapClaims(EvaluationContext evaluationContext, out IIdentity identity)
        {

            List<IIdentity> identities = evaluationContext.Properties["Identities"] as List<IIdentity>;
            
            if (identities.Count == 0)
                throw new SecurityException("Authorization failed, identity missing from evaluation context.");

            identity = identities[0];

            // TODO: check identity against credential store and 
            // determine the appropriate claims to allocate
            
            // NOTE: in this sample, only partner certificates are provided,
            // and at this point have passed authorization, so we will grant
            // all custom claims 
            
            List<Claim> listClaims = new List<Claim>();

            listClaims.Add(new Claim(CustomClaimTypes.Create, "Application", Rights.PossessProperty));
            listClaims.Add(new Claim(CustomClaimTypes.Delete, "Application", Rights.PossessProperty));
            listClaims.Add(new Claim(CustomClaimTypes.Read, "Application", Rights.PossessProperty));
            listClaims.Add(new Claim(CustomClaimTypes.Update, "Application", Rights.PossessProperty));

            return new DefaultClaimSet(this.m_issuer, listClaims);
        }

        public ClaimSet Issuer
        {
            get { return m_issuer; }
        }

        #endregion

        #region IAuthorizationComponent Members

        public string Id
        {
            get
            {
                return m_id.ToString();
            }
        }

        #endregion
    }
}
