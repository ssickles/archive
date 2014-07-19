// © 2009 Michele Leroux Bustamante. All rights reserved. 
// See http://wcfguidanceforwpf.codeplex.com for related whitepaper and updates
// For an intro to WCF see Michele's book: Learning WCF, O'Reilly 2007 (updated August 2008 for VS 2008)
// See http://www.thatindigogirl.com for the book code!

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
