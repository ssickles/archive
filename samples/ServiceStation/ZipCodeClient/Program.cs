using System;
using System.Collections.Generic;
using System.Text;
using ZipCodeClient.localhost;
using System.ServiceModel;
using Extensions;

namespace ZipCodeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the ZipCode lookup program!");
            Console.WriteLine("Enter a zipcode in the following format: #####-####");
            Console.WriteLine("Or enter 'quit' to close the program.\n");
            Console.WriteLine("Some sample zip codes: 84041-2941, 97206-6825, 85383-8718\n");

            ZipCodeServiceClient client = new ZipCodeServiceClient();

            string zipcode = Console.ReadLine();

            while (!zipcode.Equals("quit"))
            {
                try
                {
                    Console.WriteLine(client.Lookup(zipcode));
                }
                catch (FaultException fe)
                {
                    Console.WriteLine(fe.Message);
                }
                zipcode = Console.ReadLine();
            }
        }
    }
}
