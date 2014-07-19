/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Vci.Silverlight.FileUploader
{
    [ScriptableType]
    public class UploadErrorOccurredEventArgs : EventArgs
    {
        private string _errorMsg;

        [ScriptableMember]
        public string ErrorMessage { get { return _errorMsg; } }

        public UploadErrorOccurredEventArgs(string ErrorMessage)
        {
            _errorMsg = ErrorMessage;
        }
    }

    public class UploadDataSentArgs : EventArgs
    {
        public readonly long TotalDataSent;
        
        public UploadDataSentArgs(long TotalDataSent)
        {
            this.TotalDataSent = TotalDataSent;
        }
    }

    /// <summary>
    /// Interface for different kind of file uploaders
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Starts the upload; can pass an optional extra parameter that is sent to the server with each chunk.
        /// </summary>
        /// <param name="UploadedFileProcessorType"></param>
        /// <param name="ContextParam"></param>
        void StartUpload(string UploadedFileProcessorType, string ContextParam);        

        /// <summary>
        /// Fired when the upload has successfully finished.  The uploader will not access the UserFile object after this is invoked.
        /// </summary>
        event EventHandler UploadFinished;

        /// <summary>
        /// An upload has been successfully canceled.  The uploader will not access the UserFile object after this is invoked.
        /// </summary>
        event EventHandler UploadCanceled;

        /// <summary>
        /// An error has occurred.  The upload aborts, and the UserFile is not accessed after this is invoked.
        /// </summary>
        event EventHandler<UploadErrorOccurredEventArgs> UploadErrorOccurred;

        /// <summary>
        /// Called each time data is sent to the server, and provides the total amount sent so far.
        /// </summary>
        event EventHandler<UploadDataSentArgs> UploadDataSent;

        /// <summary>
        /// Post-processing of the file has started.
        /// </summary>
        event EventHandler UploadProcessingStarted;
    }
}
