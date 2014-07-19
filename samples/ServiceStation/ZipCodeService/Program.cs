using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Extensions;

namespace ZipCodeServiceLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ZipCodeService));
            try
            {
                host.Open();
                
                Console.WriteLine("ZipCodeService is running. Press any key to exit.");
                Console.ReadKey();

                host.Close(); // successful close
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                host.Abort();
            }
        }
    }
}
