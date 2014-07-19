using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WcfServiceSandbox;
using System.ServiceModel.Description;
using System.Threading;
using System.ServiceModel.Security;
using WcfBehaviorExtensionSandbox;

namespace ConsoleApplication10
{
    public class Program
    {
        private static ServiceHost host;

        static void Main(string[] args)
        {
            //var e = TestDistinctDefer();
            //foreach (var t in e)
            //{
            //    Console.WriteLine("loop:" + t);
            //}

            host = new ServiceHost(typeof(EscortService), new Uri("https://dazzle:9009/EscortService"));

            //Uri baseAddress = new Uri("https://dazzle:9009/EscortService");
            //host = new ServiceHost(typeof(EscortService), baseAddress);
            //var endpoint = host.AddServiceEndpoint(typeof(IEscortService), BuildBinding(), "");
            //GetServiceBehaviors().ToList()
            //    .ForEach(host.Description.Behaviors.Add);

            //host.Credentials.ClientCertificate.SetCertificate(
            //    System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine,
            //    System.Security.Cryptography.X509Certificates.StoreName.My,
            //    System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint,
            //    "f96538da621b4ee3fff009a29d3903fc010a4c93");

            host.Open();

            Console.WriteLine("Running...");
            Console.ReadLine();

            host.Close();
        }

        //private static IEnumerable<string> TestDistinctDefer()
        //{
        //    Console.WriteLine("A");
        //    yield return "A";
        //    Console.WriteLine("B");
        //    yield return "B";
        //    Console.WriteLine("C");
        //    yield return "C";
        //}

        //static System.ServiceModel.Channels.Binding BuildBinding()
        //{
        //    var binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
        //    binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
        //    return binding;
        //}

        //static IEnumerable<IServiceBehavior> GetServiceBehaviors()
        //{
        //    yield return BuildServiceMetadataBehavior();
        //    //yield return BuildServiceDebugBehavior();
        //    yield return BuildServiceCredentialsBehavior();
        //}

        //static IServiceBehavior BuildServiceMetadataBehavior()
        //{
        //    var behavior = new ServiceMetadataBehavior
        //    {
        //        HttpsGetEnabled = true,
        //    };
        //    behavior.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
        //    return behavior;
        //}

        //static IServiceBehavior BuildServiceDebugBehavior()
        //{
        //    return new ServiceDebugBehavior
        //    {
        //        IncludeExceptionDetailInFaults = true,
        //    };
        //}

        //static IServiceBehavior BuildServiceCredentialsBehavior()
        //{
        //    var serviceCredentials = new ServiceCredentials();
        //    serviceCredentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
        //    serviceCredentials.UserNameAuthentication.CustomUserNamePasswordValidator = new BiometricValidator();
        //    return serviceCredentials;
        //}
    }
}
