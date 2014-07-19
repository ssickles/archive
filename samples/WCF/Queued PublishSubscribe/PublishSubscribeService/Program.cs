//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.ServiceModel;
using ServiceModelEx;
using System.Messaging;

namespace QueuedPublishSubscribe
{
   static class Program
   {
      static void Main()
      {
         if(MessageQueue.Exists(@".\private$\MyEventServiceQueue") == false)
         {
            MessageQueue.Create(@".\private$\MyEventServiceQueue",true);
         }

         ServiceHost publishServiceHost = new ServiceHost(typeof(MyPublishService),new Uri("http://localhost:5000"));
         publishServiceHost.Open();

         ServiceHost subscriptionManagerHost = new ServiceHost(typeof(MySubscriptionService),new Uri("http://localhost:6000"));
         subscriptionManagerHost.Open();

         Application.Run(new HostForm());

         publishServiceHost.Close();
         subscriptionManagerHost.Close();
      }
   }
}