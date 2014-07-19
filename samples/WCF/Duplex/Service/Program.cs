//2006 IDesign Inc.  
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace DuplexDemo
{
   static class Program
   {
      static void Main()
      {
         ServiceHost host = new ServiceHost(typeof(MyService),new Uri("http://localhost:8000/"));
         host.Open();

         Application.Run(new MyForm());

         host.Close();
      }
   }
}