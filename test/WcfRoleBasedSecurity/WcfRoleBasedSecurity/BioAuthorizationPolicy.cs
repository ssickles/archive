using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.ServiceModel;
using System.IO;
using IdentityStream.Models;

namespace WcfRoleBasedSecurity
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
                        ident.AuthenticationType == "BiometricValidator")
                    {
                        StringReader reader = new StringReader(ident.Name);
                        System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(SecureContextCredentials));
                        SecureContextCredentials credentials = (SecureContextCredentials)deserializer.Deserialize(reader);
                        IdsIdentity i = new IdsIdentity(ident.IsAuthenticated, AuthenticationType.Bioemtrics, credentials.IdentityUid);
                        IdsPrincipal p = new IdsPrincipal(i, new List<string> { "LoggedIn" });
                        evaluationContext.Properties["Principal"] = p;
                        return true;
                    }
                    else
                    {
                        string userId = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("userId", "http://www.identitystream.com");
                        IdsIdentity i = new IdsIdentity(ident.IsAuthenticated, AuthenticationType.Certificate, userId);
                        IdsPrincipal p = new IdsPrincipal(i, new List<string> { "LoggedIn" });
                        evaluationContext.Properties["Principal"] = p;
                        return true;
                    }
                }
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
