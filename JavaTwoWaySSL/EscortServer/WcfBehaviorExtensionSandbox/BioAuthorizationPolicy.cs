using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.ServiceModel;

namespace WcfBehaviorExtensionSandbox
{
    public class BioAuthorizationPolicy : IAuthorizationPolicy
    {
        // called after the authentication stage
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            IList<IIdentity> idents;
            object identsObject;
            if (evaluationContext.Properties.TryGetValue(
                "Identities", out identsObject) && (idents =
                identsObject as IList<IIdentity>) != null)
            {
                foreach (IIdentity ident in idents)
                {
                    if (ident.IsAuthenticated &&
                        ident.AuthenticationType == "BioUsernamePasswordVerifier")
                    {
                        //evaluationContext.Properties["Principal"]
                        //    = new IdsPrincipal();
                        return true;
                    }
                    else
                    {
                        string userId = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("userId", "http://www.identitystream.com");
                    }
                }
            }
            if (!evaluationContext.Properties.ContainsKey("Principal"))
            {
                //evaluationContext.Properties["Principal"] = new IdsPrincipal();
            }
            return false;
        }


        public System.IdentityModel.Claims.ClaimSet Issuer
        {
            get { throw new NotImplementedException(); }
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }
    }
}
