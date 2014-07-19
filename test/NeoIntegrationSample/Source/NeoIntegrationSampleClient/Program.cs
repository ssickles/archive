using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoIntegrationSampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            BiometricsService.BiometricsService serviceClient = new NeoIntegrationSampleClient.BiometricsService.BiometricsService();
            serviceClient.VerifyCustomerCompleted += new NeoIntegrationSampleClient.BiometricsService.VerifyCustomerCompletedEventHandler(serviceClient_VerifyCustomerCompleted);
            string exit = string.Empty;

            while (exit.Length == 0)
            {
                Guid uid = Guid.NewGuid();
                string firstName = "Scott";
                string lastName = "Sickles";
                serviceClient.VerifyCustomerAsync(uid.ToString(), firstName, lastName);
                Console.WriteLine(string.Format("Sent customer verification request: ({0}) {1} {2}", uid.ToString(), firstName, lastName));
                exit = Console.ReadLine();
            }
        }

        static void serviceClient_VerifyCustomerCompleted(object sender, NeoIntegrationSampleClient.BiometricsService.VerifyCustomerCompletedEventArgs e)
        {
            if (e.Error == null)
                Console.WriteLine(string.Format("Verification response received: {0}", e.VerifyCustomerResult));
            else
                Console.WriteLine(e.Error.Message);
        }
    }
}
