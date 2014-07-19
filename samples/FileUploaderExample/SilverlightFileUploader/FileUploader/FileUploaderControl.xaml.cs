/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Linq;
using System.IO;
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
    public partial class FileUploaderControl : UserControl
    {
        private bool _multiSelect = true;
        protected FileCollection _files;

        public FileCollection Files { get { return _files; } }

        /// <summary>
        /// File filter -- see documentation for .net OpenFileDialog for details.
        /// </summary>
        public string FileFilter { get; set; }

        /// <summary>
        /// Path to the upload handler.
        /// </summary>
        public string UploadHandlerPath { get; set; }

        /// <summary>
        /// Maximum file size of an uploaded file in KB.  Defaults to no limit.
        /// </summary>
        public long MaxFileSizeKB { get; set; }

        /// <summary>
        /// Chunk size in megabytes.  Defaults to 3 MB.
        /// </summary>
        public long ChunkSizeMB { get; set; }

        /// <summary>
        /// True if multiple files can be selected.  Defaults to true.
        /// </summary>
        public bool MultiSelect
        {
            get { return _multiSelect; }
            set
            {
                _multiSelect = value;
                txtEmptyMessage.Text = _multiSelect ? "Click 'Upload Files...' and choose 1 or more files to begin uploading." :
                    "Click 'Upload Files...' and choose a file to begin uploading.";
            }
        }

        /// <summary>
        /// Access to the scroll viewer -- used by silverlight app that needs to host this in windowless mode
        /// and fix mouse capture bugs.
        /// </summary>
        public ScrollViewer ScrollViewer { get { return svFiles; } }

        /// <summary>
        /// This event is raised when the user selects 1 or more files and begins uploading.
        /// </summary>
        [ScriptableMember]
        public event EventHandler UploadStarted;

        public FileUploaderControl()
        {
            // Required to initialize variables
            InitializeComponent();

            MaxFileSizeKB = -1;
            ChunkSizeMB = 3;

            MultiSelect = true;

            _files = new FileCollection { MaxUploads = 2 };

            icFiles.ItemsSource = _files;
            progressPercent.DataContext = _files;
            txtUploadedBytes.DataContext = _files;
            txtPercent.DataContext = _files;

            this.Loaded += new RoutedEventHandler(FileUploaderControl_Loaded);

            _files.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_files_CollectionChanged);
            _files.PercentageChanged += new EventHandler(_files_PercentageChanged);
            _files.AllFilesFinished += new EventHandler(_files_AllFilesFinished);
            _files.ErrorOccurred += new EventHandler<UploadErrorOccurredEventArgs>(_files_ErrorOccurred);
        }

        void _files_ErrorOccurred(object sender, UploadErrorOccurredEventArgs e)
        {
            // not sure why this is necessary sometimes... the progress bar will get stuck after an error sometimes
            _files_PercentageChanged(sender, null);
        }

        void _files_PercentageChanged(object sender, EventArgs e)
        {
            // if the percentage is decreasing, don't use an animation
            if (_files.Percentage < progressPercent.Value)
                progressPercent.Value = _files.Percentage;
            else
            {
                sbProgressFrame.Value = _files.Percentage;
                sbProgress.Begin();
            }
        }

        void _files_AllFilesFinished(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this, "Finished", true);
        }

        void _files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_files.Count == 0)
            {
                VisualStateManager.GoToState(this, "Empty", true);
                btnChoose.IsEnabled = true;
            }
            else
            {
                // disable the upload button if only a single file can be uploaded
                btnChoose.IsEnabled = MultiSelect;

                if (_files.FirstOrDefault(f => !f.IsCompleted) != null)
                    VisualStateManager.GoToState(this, "Uploading", true);
                else
                    VisualStateManager.GoToState(this, "Finished", true);
            }
        }

        private void FileUploaderControl_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Empty", false);
        }

        /// <summary>
        /// Initialize this control from an InitParams collection provided to the silverlight plugin.  Also sets this control up
        /// to be accessed via javascript.
        /// </summary>
        /// <param name="InitParams"></param>
        public void InitializeFromInitParams(IDictionary<string, string> initParams)
        {
            HtmlPage.RegisterScriptableObject("UploaderControl", this);
            HtmlPage.RegisterScriptableObject("Files", _files);

            if (initParams.ContainsKey("UploadedFileProcessorType") && !string.IsNullOrEmpty(initParams["UploadedFileProcessorType"]))
                _files.UploadedFileProcessorType = initParams["UploadedFileProcessorType"];

            if (initParams.ContainsKey("MaxUploads") && !string.IsNullOrEmpty(initParams["MaxUploads"]))
                _files.MaxUploads = int.Parse(initParams["MaxUploads"]);

            if (initParams.ContainsKey("MaxFileSizeKB") && !string.IsNullOrEmpty(initParams["MaxFileSizeKB"]))
                MaxFileSizeKB = long.Parse(initParams["MaxFileSizeKB"]);

            if (initParams.ContainsKey("ChunkSizeMB") && !string.IsNullOrEmpty(initParams["ChunkSizeMB"]))
                ChunkSizeMB = long.Parse(initParams["ChunkSizeMB"]);

            if (initParams.ContainsKey("FileFilter") && !string.IsNullOrEmpty(initParams["FileFilter"]))
                FileFilter = initParams["FileFilter"];

            if (initParams.ContainsKey("UploadHandlerPath") && !string.IsNullOrEmpty(initParams["UploadHandlerPath"]))
                UploadHandlerPath = initParams["UploadHandlerPath"];

            if (initParams.ContainsKey("MultiSelect") && !string.IsNullOrEmpty(initParams["MultiSelect"]))
                MultiSelect = Convert.ToBoolean(initParams["MultiSelect"].ToLower());
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            SelectUserFiles();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFileList();
        }

        /// <summary>
        /// Open the select file dialog, and begin uploading files.
        /// </summary>
        private void SelectUserFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = MultiSelect;

            try
            {
                // Check the file filter (filter is used to filter file extensions to select, for example only .jpg files)
                if (!string.IsNullOrEmpty(FileFilter))
                    ofd.Filter = FileFilter;
            }
            catch (ArgumentException ex)
            {
                // User supplied a wrong configuration filter
                throw new Exception("Wrong file filter configuration.", ex);
            }

            if (ofd.ShowDialog() == true)
            {
                foreach (FileInfo file in ofd.Files)
                {
                    // create an object that represents a file being uploaded
                    UserFile userFile = new UserFile();
                    userFile.FileName = file.Name;
                    userFile.FileStream = file.OpenRead();
                    userFile.UIDispatcher = this.Dispatcher;
                    userFile.HttpUploader = true;
                    userFile.UploadHandlerName = UploadHandlerPath;
                    userFile.ChunkSizeMB = ChunkSizeMB;

                    // check the file size limit
                    if (MaxFileSizeKB == -1 || userFile.FileSize <= MaxFileSizeKB * 1024)
                        _files.Add(userFile);
                    else
                        HtmlPage.Window.Alert(userFile.FileName + " exceeds the maximum file size of " + (MaxFileSizeKB).ToString() + "KB.");
                }
            }

            // start uploading the selected files
            if (_files.Count > 0)
            {
                OnUploadStarted();
                _files.UploadFiles();
            }
        }

        /// <summary>
        /// Clear the file list
        /// </summary>
        [ScriptableMember]
        public void ClearFileList()
        {
            // clear all files that are completed (canceled, finished, error), or pending
            _files.Where(f => f.IsCompleted || f.State == Constants.FileStates.Pending).ToList().ForEach(f => _files.Remove(f));
        }

        private void OnUploadStarted()
        {
            EventHandler handler = UploadStarted;
            if (handler != null)
                handler(this, null);
        }
    }

    public class PercentConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string percent = "0%";

            if (value != null)
                percent = (int)value + "%";

            return percent;
        }

        // only use one-way binding for percentages
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}