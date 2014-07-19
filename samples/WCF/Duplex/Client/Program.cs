//2006 IDesign Inc.  
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Diagnostics;
using System.Threading;

namespace Client
{
   static class Program
   {
      static void Main()
      {
         Trace.WriteLine("Client thread = " + Thread.CurrentThread.ManagedThreadId);
         MyClient client = new MyClient();
         InstanceContext context = new InstanceContext(client);
         MyContractClient proxy = new MyContractClient(context);
 
         proxy.DoSomething();

         proxy.Close();
      }
   }
}