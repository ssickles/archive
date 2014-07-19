/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Vci.FileUploader
{
    /// <summary>
    /// Interface for classes that can be used as the UploadedFileProcessorType on UploaderControl2.
    /// When a file has been completely uploaded, if an UploadedFileProcessorType has been set on the uploader web control,
    /// the ProcessFile method will be called, which gives custom code an opportunity to process the file.
    /// </summary>
    public interface IUploadedFileProcessor
    {
        /// <summary>
        /// Process an uploaded file.
        /// </summary>
        /// <param name="Context">http context for the request</param>
        /// <param name="FileGuid">
        /// FileGuid that uniquely identifies the uploaded file.
        /// </param>
        /// <param name="FileName">File name (does not include any path information, which is security critical in silverlight)</param>
        /// <param name="ContextParam">
        /// A string with context information.  Corresponds to the UserContextParameter property of the FileCollection, which is
        /// exposed via the setUserContext() js method on the UploaderControl javascript wrapper class.
        /// </param>
        void ProcessFile(HttpContext Context, string FileGuid, string FileName, string ContextParam);
    }
}
