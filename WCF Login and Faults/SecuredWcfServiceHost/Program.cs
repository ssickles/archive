using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using SecuredWcfService;

namespace SecuredWcfServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(TestService));
            host.Open();
            Console.WriteLine("Test Service Hosted.");
            Console.ReadLine();
        }
    }
}
