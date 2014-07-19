//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.ServiceModel;

namespace PubSubService
{
   static class Program
   {
      static void Main()
      {
         Application.Run(new MyPublisherForm());
      }
   }
}