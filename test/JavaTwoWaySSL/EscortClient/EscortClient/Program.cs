using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using EscortClient.EscortServiceReference;
using System.ServiceModel.Channels;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace EscortClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;

            var client = new EscortServiceClient("WSHttpBinding_IEscortService");

            //var address = new EndpointAddress("https://dazzle.:9009/EscortService");
            //var client = new EscortServiceClient(BuildBinding(), address);
            //client.ClientCredentials.UserName.UserName = Guid.Empty.ToString();
            //client.ClientCredentials.UserName.UserName = Guid.NewGuid().ToString();
            client.Open();

            var isSlut = client.Escort(10);
            var isMonogamous = !client.Escort(1);

            client.Close();
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        //static Binding BuildBinding()
        //{
        //    var binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential, false);

        //    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
        //    binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
        //    binding.Security.Transport.Realm = "";

        //    binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
        //    binding.Security.Message.NegotiateServiceCredential = true;

        //    return binding;
        //}
    }
}
