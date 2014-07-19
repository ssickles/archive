/// Notice : Code written by Dimitris Papadimitriou - http://www.papadi.gr
/// Code is provided to be used freely but without any warranty of any kind
using System;
using System.ServiceModel;

namespace ConsoleHost
{
    class Program
    {
        static void Main()
        {
            ServiceHost myServiceHost = new ServiceHost(typeof(FileService.FileTransferService));
            myServiceHost.Open();

            Console.WriteLine("This is the SERVER console");
            Console.WriteLine("Service Started!");
            foreach (Uri address in myServiceHost.BaseAddresses)
                Console.WriteLine("Listening on " + address);
            Console.WriteLine("Click any key to close...");
            Console.ReadKey();

            myServiceHost.Close();
        }
    }
}
