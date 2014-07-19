//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace QueuedPublishSubscribe
{
   public partial class MyPublisherForm : Form
   {
      public MyPublisherForm()
      {
         InitializeComponent();
      }
      string GetEndpointName()
      {
         if(m_QueuedCheckBox.Checked)
         {
            return "MSMQ";
         }
         else
         {
            return "TCP";
         }        
      }
      void OnFireEvent(object sender,EventArgs e)
      {
         string endpointName = GetEndpointName();
         MyEventsClient proxy = new MyEventsClient(endpointName);

         if(m_Event1RadioButton.Checked)
         {
            proxy.OnEvent1();
         }

         if(m_Event2RadioButton.Checked)
         {
            proxy.OnEvent2(42);
         }

         if(m_Event3RadioButton.Checked)
         {
            proxy.OnEvent3(42,"Hello");
         }
      }
   }
}