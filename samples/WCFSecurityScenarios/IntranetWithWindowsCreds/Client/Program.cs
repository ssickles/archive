// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (localhost.HelloIndigoServiceClient proxy = new Client.localhost.HelloIndigoServiceClient())
            {
                NetworkCredential creds = new NetworkCredential("username", "password", "domain");

                proxy.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Identification;
                proxy.ClientCredentials.Windows.AllowNtlm = true;
                
                // TODO: before uncommenting this next line, 
                // set the NetworkCredential to the correct values 
                // for your workgroup or domain

                //proxy.ClientCredentials.Windows.ClientCredential = creds;

                string s = proxy.HelloIndigo("Hello from Client");
                Console.WriteLine(s);
                Console.ReadLine();
            }


        }
    }
}
