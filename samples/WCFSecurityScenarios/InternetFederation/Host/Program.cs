// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
   
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(MediaServicesLib.MediaServices)))
            {
                host.Open();

                Console.WriteLine("Service is listening...");
                Console.WriteLine();
                Console.WriteLine("Number of base addresses: {0}", host.BaseAddresses.Count);
                foreach (Uri uri in host.BaseAddresses)
                {
                    Console.WriteLine("\t{0}", uri.AbsoluteUri);
                }

                Console.WriteLine();
                Console.WriteLine("Number of channel dispatchers (listeners): {0}", host.ChannelDispatchers.Count);
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Console.WriteLine("\t{0}, {1}", dispatcher.Listener.Uri, dispatcher.BindingName);
                }

                Console.WriteLine();
                Console.WriteLine("Press <ENTER> to terminate Host");
                Console.ReadLine();

            }
        }
    }
}
