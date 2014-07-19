using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(TodoList.TodoListService)))
            {
                host.Faulted += new EventHandler(host_Faulted);
                host.Open();

                Console.WriteLine("ServiceHost now running.");

                Console.ReadLine();
            }

            Console.ReadLine();
        }

        static void host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("ServiceHost has faulted. Restart the service.");
        }
    }
}
