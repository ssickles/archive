//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.ServiceModel;
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
         Application.Run(new MyPublisherForm());
      }
   }
}