/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Vci.Core;

namespace Vci.FileUploader
{
    /// <summary>
    /// Example uploaded file processor.  Typically you would do something useful here, like place the document into a database,
    /// or create thumbnails from an uploaded image, or whatever is appropriate.  This example just deletes the file from
    /// the sandbox folder to prevent files from accumulating during testing.
    /// </summary>
    public class ExampleFileProcessor : IUploadedFileProcessor
    {
        #region IUploadedFileProcessor Members

        public void ProcessFile(HttpContext Context, string FileGuid, string FileName, string ContextParam)
        {
            string sandboxPath = Path.Combine(Sandbox.UploaderControlSandboxPath, FileGuid);

            using (FileStream fs = File.OpenRead(sandboxPath))
            {
                // do something useful to the file
            }

            // for testing, this example just deletes the file from the sandbox after it has been uploaded; comment
            // out this line if you want to verify that files are being uploaded correctly while testing
            File.Delete(sandboxPath);
        }

        #endregion
    }
}
