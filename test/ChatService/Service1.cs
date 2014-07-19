using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace ChatService
{
    public partial class Service1 : ServiceBase
    {
        ServiceHost host;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(typeof(ChatService));
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }
}
