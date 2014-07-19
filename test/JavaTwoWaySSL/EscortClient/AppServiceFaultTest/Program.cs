using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppServiceFaultTest.SecuredAppService;

namespace AppServiceFaultTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SecuredAppService.SecuredApplicationServiceClient proxy = new SecuredAppService.SecuredApplicationServiceClient();
            CountryData[] data = proxy.GetCountries(new RequestData());
            Console.WriteLine("Done fetching from the service.");
            Console.ReadLine();
        }
    }
}
