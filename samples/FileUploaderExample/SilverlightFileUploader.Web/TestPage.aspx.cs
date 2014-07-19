﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SilverlightFileUploader.Web
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected string _sandbox;

        protected void Page_Load(object sender, EventArgs e)
        {
            _sandbox = ConfigurationSettings.AppSettings["SandboxPath"];
        }
    }
}
