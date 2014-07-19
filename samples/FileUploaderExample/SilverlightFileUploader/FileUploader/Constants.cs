/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Vci.Silverlight.FileUploader
{
    public static class Constants
    {
        /// <summary>
        /// Possible States
        /// </summary>
        public enum FileStates
        {
            Pending = 0,    // has been added, upload has not begun
            Uploading = 1,  // being uploaded
            Finished = 2,   // finished uploading
            Canceled = 3,   // upload successfully canceled
            Error = 4,      // error during any server communication
            Canceling = 5,  // attempting to cancel the current upload
            Processing = 6, // performing post-processing on the file
        }
    }
}
