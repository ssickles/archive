/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Browser;

using Vci.Core;

namespace Vci.Silverlight.FileUploader
{
    /// <summary>
    /// Event arguments for the file uploaded event.
    /// </summary>
    [ScriptableType]
    public class FileUploadedEventArgs : EventArgs
    {
        private string _fileGuid;
        private string _fileName;
        private double _fileSize;

        [ScriptableMember]
        public string FileGuid { get { return _fileGuid; } }

        [ScriptableMember]
        public string FileName { get { return _fileName; } }

        [ScriptableMember]
        public double FileSize { get { return _fileSize; } }

        public FileUploadedEventArgs(UserFile file) : this(file.Guid, file.FileName, file.FileSize) { }

        public FileUploadedEventArgs(string FileGuid, string FileName, double FileSize)
        {
            _fileGuid = FileGuid;
            _fileName = FileName;
            _fileSize = FileSize;
        }
    }

    /// <summary>
    /// Collection of files loaded into the uploader control.
    /// </summary>
    [ScriptableType]
    public class FileCollection : ObservableCollection<UserFile>
    {
        private long _bytesUploaded = 0;
        private int _percentage = 0;
        private int _currentUpload = 0;
        private int _totalUploadedFiles = 0;

        /// <summary>
        /// Maximum allowable concurrent uploads; defaults to 2.
        /// </summary>
        public int MaxUploads { get; set; }

        /// <summary>
        /// A custom parameter that is sent to the file upload handler on the server.  The UserContextParameter
        /// property should be used for per-upload context information; this is meant to send static information
        /// required by the particular upload handler implementation.
        /// </summary>
        public string UploadedFileProcessorType { get; set; }

        /// <summary>
        /// A custom parameter that is sent to the file upload handler on the server.  This parameter allows users
        /// of this control to provide a context for the upload, e.g. a guid or identifier.
        /// </summary>
        [ScriptableMember]
        public string UserContextParameter { get; set; }

        /// <summary>
        /// Total number of bytes uploaded.
        /// </summary>
        public long BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("BytesUploaded"));
            }
        }

        /// <summary>
        /// Total number of files in the collection.  This method is only present to expose the count as a scriptable member.
        /// </summary>
        [ScriptableMember()]
        public int TotalFilesSelected
        {
            get { return this.Items.Count; }
        }

        /// <summary>
        /// Total completion percentage for the upload of all files in this collection.
        /// </summary>
        [ScriptableMember()]
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));
                OnPercentageChanged();
            }
        }

        public event EventHandler PercentageChanged;

        protected void OnPercentageChanged()
        {
            EventHandler handler = PercentageChanged;
            if (handler != null)
                handler(this, null);
        }

        /// <summary>
        /// Total number of successfully uploaded files.
        /// </summary>
        [ScriptableMember()]
        public int TotalUploadedFiles
        {
            get { return _totalUploadedFiles; }
            set
            {
                _totalUploadedFiles = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("TotalUploadedFiles"));
            }
        }

        [ScriptableMember]
        public event EventHandler<FileUploadedEventArgs> SingleFileUploadFinished;

        [ScriptableMember]
        public event EventHandler AllFilesFinished;

        [ScriptableMember]
        public event EventHandler<UploadErrorOccurredEventArgs> ErrorOccurred;

        /// <summary>
        /// FileCollection constructor
        /// </summary>
        /// <param name="customParams"></param>
        /// <param name="maxUploads"></param>
        public FileCollection()
        {
            MaxUploads = 2;

            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FileCollection_CollectionChanged);
        }

        /// <summary>
        /// Add a new file to the file collection
        /// </summary>
        /// <param name="item"></param>
        public new void Add(UserFile item)
        {
            // Listen to the property changed for each added item
            item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            base.Add(item);
        }

        /// <summary>
        /// Upload files.  This method will upload a maximum of _maxUpload files concurrently.  Whenever a file is finished,
        /// an upload is canceled, or an error occurs, this method is called again, until there are no more files left.
        /// </summary>
        public void UploadFiles()
        {
            foreach (UserFile file in this)
            {
                if (!file.IsDeleted && file.State == Constants.FileStates.Pending && _currentUpload < MaxUploads)
                {
                    file.Upload(UploadedFileProcessorType, UserContextParameter);
                    _currentUpload++;
                }
            }
        }

        /// <summary>
        /// Recount statistics.
        /// </summary>
        private void RecountTotal()
        {
            long totalSize = 0;
            long totalSizeDone = 0;

            foreach (UserFile file in this.Items.Where(f => f.State != Constants.FileStates.Canceled && f.State != Constants.FileStates.Error))
            {
                totalSize += file.FileSize;
                totalSizeDone += file.BytesUploaded;
            }
            BytesUploaded = totalSizeDone;

            double percentage = 0;
            if (totalSize > 0 && totalSizeDone > 0)
                percentage = 100.0 * (double)totalSizeDone / (double)totalSize;

            // The completion percent will go up smoothly as bytes are read from the file, but the server might do some processing
            // that takes longer.  Do not let the completion percentage go to 100% until every file is in a finished state.
            // Subtract 5% from the completion percent for each non-finished file.
            int unfinished = this.Items.Count(f => !f.IsCompleted);

            if (percentage > 100 - (5 * unfinished))
            {
                percentage = 100 - (5 * unfinished);
                // this would be rare to have 20 files processing simultaneously, but make sure it doesn't go below 0%
                if (percentage < 0)
                    percentage = 0;
            }

            Percentage = (int)percentage;
        }

        /// <summary>
        /// The collection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Recount total when the collection changed (items added or deleted)
            RecountTotal();
        }

        protected override void RemoveItem(int index)
        {
            // do clean-up when an item is removed from the collection
            PrepareItemForRemoval(this[index]);

            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            // do clean-up when an item is removed from the collection
            foreach (UserFile f in this)
                PrepareItemForRemoval(f);

            base.ClearItems();
        }

        private void PrepareItemForRemoval(UserFile file)
        {
            file.PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
            if (file.FileStream != null)
                file.FileStream.Dispose();
        }

        /// <summary>
        /// Property of individual item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if the IsDeleted property is set, that signals to this object to remove it from the collection;
            // note: the UI probably shouldn't allow a file to be deleted while it is still uploading
            if (e.PropertyName == "IsDeleted")
            {
                UserFile file = (UserFile)sender;
                if (file.IsDeleted)
                {
                    this.Remove(file);
                    file = null;
                }
            }
            else if (e.PropertyName == "State")
            {
                // Finished, Error, and Canceled terminate the attempt to upload a file.  Decrement the uploading count,
                // and call UploadFiles to check for other pending uploads.

                UserFile file = (UserFile)sender;
                if (file.State == Constants.FileStates.Finished)
                {
                    // if there is no post-processor for files, we wait until the finished state to free up another slot
                    // for uploading files; if there is one, when the file moves to the Processing state below, we free up a slot.
                    if (string.IsNullOrEmpty(UploadedFileProcessorType))
                        _currentUpload--;

                    TotalUploadedFiles++;

                    UploadFiles();

                    OnSingleFileUploadFinished(new FileUploadedEventArgs(file));

                    RecountTotal();
                    AreAllFilesFinished();
                }
                else if (file.State == Constants.FileStates.Processing)
                {
                    // the file is being processed so it's not quite finished, but we're free to start uploading another file
                    _currentUpload--;

                    UploadFiles();

                    RecountTotal();
                }
                else if (file.State == Constants.FileStates.Error)
                {
                    _currentUpload--;
                    UploadFiles();

                    RecountTotal();

                    OnErrorOccurred(new UploadErrorOccurredEventArgs(file.ErrorMessage));
                }
                else if (file.State == Constants.FileStates.Canceled)
                {
                    // an upload was successfully canceled
                    _currentUpload--;
                    UploadFiles();

                    RecountTotal();
                }
                else
                    RecountTotal();
            }
            else if (e.PropertyName == "BytesUploaded")
                RecountTotal();
        }

        /// <summary>
        /// Check if all files are finished uploading
        /// </summary>
        private void AreAllFilesFinished()
        {
            if (Percentage == 100)
                OnAllFilesFinished();
        }

        private void OnAllFilesFinished()
        {
            EventHandler handler = AllFilesFinished;
            if (handler != null)
                handler(this, null);
        }

        private void OnSingleFileUploadFinished(FileUploadedEventArgs e)
        {
            EventHandler<FileUploadedEventArgs> handler = SingleFileUploadFinished;
            if (handler != null)
                handler(this, e);
        }

        private void OnErrorOccurred(UploadErrorOccurredEventArgs e)
        {
            EventHandler<UploadErrorOccurredEventArgs> handler = ErrorOccurred;
            if (handler != null)
                handler(this, e);
        }
    }
}
