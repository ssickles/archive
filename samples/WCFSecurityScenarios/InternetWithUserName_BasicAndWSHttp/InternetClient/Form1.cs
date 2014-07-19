// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InternetClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (localhost.HelloIndigoServiceClient soap12proxy = new InternetClient.localhost.HelloIndigoServiceClient("WSHttpBinding_IHelloIndigoService"))
            {
                soap12proxy.ClientCredentials.UserName.UserName = "Admin";
                soap12proxy.ClientCredentials.UserName.Password = "p@ssw0rd";
                string s = soap12proxy.HelloIndigo("Soap 1.2 endpoint using WSHttpBinding.");
                MessageBox.Show(s);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (localhost.HelloIndigoServiceClient soap11proxy = new InternetClient.localhost.HelloIndigoServiceClient("BasicHttpBinding_IHelloIndigoService"))
            {
                soap11proxy.ClientCredentials.UserName.UserName = "Admin";
                soap11proxy.ClientCredentials.UserName.Password = "p@ssw0rd";
                string s = soap11proxy.HelloIndigo("Soap 1.1 endpoint using BasicHttpBinding.");
                MessageBox.Show(s);
            }

        }
    }
}