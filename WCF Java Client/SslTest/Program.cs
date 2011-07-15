using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SslTest.ServiceReference1;
using SslTest.local.hyperion.scottslaptop;

namespace SslTest
{
    class Program
    {
        static void Main(string[] args)
        {
            WSStyle();
        }

        static void WcfStyle()
        {
            ApplicationServiceClient appService = new ApplicationServiceClient();
            SslTest.ServiceReference1.ApplicationTypeData[] data = appService.GetApplicationTypes(new SslTest.ServiceReference1.RequestData());
            foreach (SslTest.ServiceReference1.ApplicationTypeData d in data)
            {
                Console.WriteLine(d.Description);
            }
            Console.ReadLine();
        }

        static void WSStyle()
        {
            ApplicationService appService = new ApplicationService();
            SslTest.local.hyperion.scottslaptop.ApplicationTypeData[] data = appService.GetApplicationTypes(new SslTest.local.hyperion.scottslaptop.RequestData());
            foreach (SslTest.local.hyperion.scottslaptop.ApplicationTypeData d in data)
            {
                Console.WriteLine(d.Description);
            }
            Console.ReadLine();
        }
    }
}
