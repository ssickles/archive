using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;

namespace WcfBehaviorExtensionSandbox
{
    class CustomCertValidator : X509CertificateValidator
    {
        public override void Validate(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
        {
            X509Store store = new X509Store("TrustedPeople", StoreLocation.LocalMachine);
            var foundCert = store.Certificates.Find(X509FindType.FindByThumbprint, certificate.Thumbprint, true).OfType<X509Certificate>().FirstOrDefault();
            if (foundCert == null)
                throw new SecurityTokenValidationException("The certificate provided by the client is not trusted.");
        }
    }
}
