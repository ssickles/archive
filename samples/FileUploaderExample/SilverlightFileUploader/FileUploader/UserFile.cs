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
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;

using Vci.Core;

namespace Vci.Silverlight.FileUploader
{
    /// <summary>
    /// Synchronization state for the UserFile.
    /// </summary>
    public class UserFileSyncState
    {
        public bool CancelRequested { get; set; }
    }

    public class UserFile : INotifyPropertyChanged
    {
        public UserFileSyncState SyncState = new UserFileSyncState();

        private Constants.FileStates _state;
        private string _fileName;
        private Stream _fileStream;
        private long _bytesUploaded = 0;
        private long _fileSize = 0;
        private int _percentage = 0;
        private IFileUploader _fileUploader;
        private string _guid;
        private bool _isDeleted;

        public UserFile()
        {
            _guid = System.Guid.NewGuid().ToString("N").ToLower();
            this.State = Constants.FileStates.Pending;
        }

        /// <summary>
        /// Guid that uniquely identifies this file.
        /// </summary>
        public string Guid { get { return _guid; } }

        /// <summary>
        /// Reference to UI Dispatcher.
        /// </summary>
        public Dispatcher UIDispatcher { get; set; }

        /// <summary>
        /// This is always set to true currently, as http upload is the only supported method right now.
        /// </summary>
        public bool HttpUploader { get; set; }

        /// <summary>
        /// Full path to the upload handler (excluding scheme, host, port).
        /// </summary>
        public string UploadHandlerName { get; set; }

        /// <summary>
        /// Chunk size for uploading the file.
        /// </summary>
        public long ChunkSizeMB { get; set; }

        /// <summary>
        /// File name, which does not include user's local path info (path info is security critical).
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    NotifyPropertyChanged("FileName");
                }
            }
        }

        /// <summary>
        /// The file stream with the data on the user's local system.
        /// </summary>
        public Stream FileStream
        {
            get { return _fileStream; }
            set
            {
                _fileStream = value;
                if (_fileStream != null)
                    _fileSize = _fileStream.Length;
            }
        }

        /// <summary>
        /// Size of the file, set when the FileStream is set.
        /// </summary>
        public long FileSize { get { return _fileSize; } }

        /// <summary>
        /// Total number of bytes uploaded for this file.
        /// </summary>
        public long BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                if (_bytesUploaded != value)
                {
                    _bytesUploaded = value;
                    NotifyPropertyChanged("BytesUploaded");
                    Percentage = (int)((value * 100) / FileSize);
                }
            }
        }

        /// <summary>
        /// Current completion percentage.
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                if (_percentage != value)
                {
                    _percentage = value;
                    NotifyPropertyChanged("Percentage");
                }
            }
        }

        /// <summary>
        /// If an error occurs while uploading this file, the message is available via this property.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Current state of the file
        /// </summary>
        public Constants.FileStates State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// Returns true if this file is in one of the completed states: Finished, Canceled, or Error.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return _state == Constants.FileStates.Finished || _state == Constants.FileStates.Canceled || _state == Constants.FileStates.Error;
            }
        }

        /// <summary>
        /// Set to true to mark this file as deleted.  The FileCollection will monitor this property and remove the file
        /// from its collection when this is set to true.
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                if (_isDeleted != value)
                {
                    _isDeleted = value;
                    NotifyPropertyChanged("IsDeleted");
                }
            }
        }

        /// <summary>
        /// Upload the file.
        /// </summary>
        /// <param name="UploadedFileProcessorType">type name for a class that will do post-processing on the uploaded files</param>
        /// <param name="ContextParam">Extra context parameter that is passed to the http handler</param>
        public void Upload(string UploadedFileProcessorType, string ContextParam)
        {
            // if we're already uploading, don't do anything
            if (this.State == Constants.FileStates.Uploading)
                return;

            this.State = Constants.FileStates.Uploading;

            if (HttpUploader)
                _fileUploader = new HttpFileUploader(this, UploadHandlerName, ChunkSizeMB);
            // for now use only the http uploader method
            //else
            //    _fileUploader = new WcfFileUploader(this);

            _fileUploader.UploadFinished += new EventHandler(fileUploader_UploadFinished);
            _fileUploader.UploadCanceled += new EventHandler(fileUploader_UploadCanceled);
            _fileUploader.UploadDataSent += new EventHandler<UploadDataSentArgs>(fileUploader_UploadDataSent);
            _fileUploader.UploadErrorOccurred += new EventHandler<UploadErrorOccurredEventArgs>(fileUploader_UploadErrorOccurred);
            _fileUploader.UploadProcessingStarted += new EventHandler(fileUploader_UploadProcessingStarted);

            _fileUploader.StartUpload(UploadedFileProcessorType, ContextParam);
        }

        /// <summary>
        /// Cancel the upload of this file.
        /// </summary>
        public void CancelUpload()
        {
            if (_fileUploader != null && this.State == Constants.FileStates.Uploading)
            {
                lock (this.SyncState)
                {
                    this.SyncState.CancelRequested = true;
                }

                this.State = Constants.FileStates.Canceling;
            }
        }

        private void fileUploader_UploadFinished(object sender, EventArgs e)
        {
            UIDispatcher.BeginInvoke(delegate
            {
                // close the file stream; this file will never be accessed again with this object
                this.FileStream.Dispose();

                _fileUploader = null;
                this.State = Constants.FileStates.Finished;
            });
        }

        private void fileUploader_UploadCanceled(object sender, EventArgs e)
        {
            UIDispatcher.BeginInvoke(delegate
            {
                // leave the file stream open if an upload is canceled, in case the UI allows re-sending

                _fileUploader = null;
                this.State = Constants.FileStates.Canceled;
            });
        }

        private void fileUploader_UploadErrorOccurred(object sender, UploadErrorOccurredEventArgs e)
        {
            UIDispatcher.BeginInvoke(delegate
            {
                // leave the file stream open if an upload caused an error, in case the UI allows re-sending

                _fileUploader = null;
                this.ErrorMessage = e.ErrorMessage;
                this.State = Constants.FileStates.Error;
            });
        }

        private void fileUploader_UploadDataSent(object sender, UploadDataSentArgs e)
        {
            UIDispatcher.BeginInvoke(delegate
            {
                this.BytesUploaded = e.TotalDataSent;
            });
        }

        private void fileUploader_UploadProcessingStarted(object sender, EventArgs e)
        {
            UIDispatcher.BeginInvoke(delegate
            {
                this.State = Constants.FileStates.Processing;
            });
        }

        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
