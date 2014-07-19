//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ServiceModel;

partial class MyPersistentSubscriber : IMyEvents
{
   public void OnEvent1()
   {
      MessageBox.Show("OnEvent1()","MyPersistentSubscriber",MessageBoxButtons.OK,MessageBoxIcon.None,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
   }
   public void OnEvent2(int number)
   {
      MessageBox.Show("OnEvent2()","MyPersistentSubscriber",MessageBoxButtons.OK,MessageBoxIcon.None,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
   }
   public void OnEvent3(int number,string text)
   {
      MessageBox.Show("OnEvent3()","MyPersistentSubscriber",MessageBoxButtons.OK,MessageBoxIcon.None,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
   }
}


     