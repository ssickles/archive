/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Vci.Core
{
    public class Sandbox
    {
        /// <summary>
        /// This folder must have the proper permissions for a web application to write to it.  Either ASPNET or NETWORK SERVICE
        /// user needs write access, depending on your OS/IIS version.
        /// </summary>
        public static string UploaderControlSandboxPath
        {
            get
            {
                string path = Path.Combine(ConfigurationSettings.AppSettings["SandboxPath"], "UploaderControl");

                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("An error occurred while trying to create a directory on disk for storing your uploaded files. " +
                            "Make sure that you have set the app setting 'SandboxPath' in web.config appropriately, and that the sandbox " +
                            "path has the appropriate permissions.", ex);
                    }
                }

                return path;
            }
        }
    }
}
