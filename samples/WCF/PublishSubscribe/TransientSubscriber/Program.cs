//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ServiceModel;

namespace Subscriber
{
   static class Program
   {
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.Run(new MyTransientSubscriber());
      }
   }
}