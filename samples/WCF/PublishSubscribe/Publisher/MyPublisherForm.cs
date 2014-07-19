//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace PubSubService
{
   public partial class MyPublisherForm : Form
   {
      public MyPublisherForm()
      {
         InitializeComponent();
      }
      void OnFireEvent(object sender,EventArgs e)
      {
         MyEventsProxy proxy = new MyEventsProxy();

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