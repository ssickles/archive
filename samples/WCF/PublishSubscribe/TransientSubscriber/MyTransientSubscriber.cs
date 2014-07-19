//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ServiceModel;

namespace Subscriber
{
   partial class MyTransientSubscriber : Form,IMyEvents
   {
      MySubscriptionServiceProxy m_Proxy;

      public MyTransientSubscriber()
      {
         InitializeComponent();
         InstanceContext context = new InstanceContext(this);

         m_Proxy = new MySubscriptionServiceProxy(context);
      }
      string GetSelectedMethod()
      {
         if(m_Event1RadioButton.Checked)
         {
            return "OnEvent1";
         }
         if(m_Event2RadioButton.Checked)
         {
            return "OnEvent2";
         }
         if(m_Event3RadioButton.Checked)
         {
            return "OnEvent3";
         }
         if(m_AllEventsRadioButton.Checked)
         {
            return "";
         }
         throw new InvalidOperationException();
      }
      public void OnEvent1()
      {
         MessageBox.Show("OnEvent1()","MyTransientSubscriber");
      }
      public void OnEvent2(int number)
      {
         MessageBox.Show("OnEvent2()","MyTransientSubscriber");
      }
      public void OnEvent3(int number,string text)
      {
         MessageBox.Show("OnEvent3()","MyTransientSubscriber");
      }
      void OnClosing(object sender,FormClosingEventArgs e)
      {
         m_Proxy.Close();
      }
      void OnSubscribe(object sender,EventArgs e)
      {
         m_Proxy.Subscribe(GetSelectedMethod());
      }
      void OnUnsubscribe(object sender,EventArgs e)
      {
         m_Proxy.Unsubscribe(GetSelectedMethod());
      }
   }
}