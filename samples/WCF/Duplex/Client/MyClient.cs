//2006 IDesign Inc.  
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
   public partial class MyClient : IMyContractCallback
   {
      public void OnCallback()
      {
         Debug.Assert(Thread.CurrentThread.IsThreadPoolThread);
         Trace.WriteLine("Callback thread = " + Thread.CurrentThread.ManagedThreadId);

         MessageBox.Show("OnCallback()","MyClient");
      }

   }
}