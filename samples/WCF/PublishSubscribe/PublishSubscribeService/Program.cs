//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.ServiceModel;
using PubSubService;

static class Program
{
   static void Main()
   {
      ServiceHost publishServiceHost = new ServiceHost(typeof(MyPublishService),new Uri("http://localhost:8008/"));
      publishServiceHost.Open();

      ServiceHost subscriptionManagerHost = new ServiceHost(typeof(MySubscriptionService),new Uri("http://localhost:8009/"));
      subscriptionManagerHost.Open();

      Application.Run(new HostForm());

      publishServiceHost.Close();
      subscriptionManagerHost.Close();
   }
}
