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
         if(MessageQueue.Exists(@".\private$\MyPersistentSubscriberQueue1") == false)
         {
            MessageQueue.Create(@".\private$\MyPersistentSubscriberQueue1",true);
         }
         if(MessageQueue.Exists(@".\private$\MyPersistentSubscriberQueue2") == false)
         {
            MessageQueue.Create(@".\private$\MyPersistentSubscriberQueue2",true);
         }
         if(MessageQueue.Exists(@".\private$\MyPersistentSubscriberQueue3") == false)
         {
            MessageQueue.Create(@".\private$\MyPersistentSubscriberQueue3",true);
         }
         ServiceHost host1 = new ServiceHost(typeof(MyPersistentSubscriber1),new Uri("http://localhost:7000/"));
         host1.Open();

         ServiceHost host2 = new ServiceHost(typeof(MyPersistentSubscriber2),new Uri("http://localhost:8000/"));
         host2.Open();

         ServiceHost host3 = new ServiceHost(typeof(MyPersistentSubscriber3),new Uri("http://localhost:9000/"));
         host3.Open();

         Application.Run(new HostForm());

         host1.Close();
         host2.Close();
         host3.Close();
      }
   }
}