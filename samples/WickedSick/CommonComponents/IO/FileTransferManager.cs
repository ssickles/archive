using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using System.Net;

namespace WickedSick.CommonComponents.IO
{
    delegate void FileTransferChange(FileTransfer Transfer);
    delegate void StartUpload();

    public class FileTransferManager : IDisposable
    {
        private ObservableCollection<FileTransfer> _files;
        private FileTransfer _curTransfer;
        private Dispatcher _entryDispatcher;
        private event FileTransferChange _fileEnqueuedEvent;
        private event FileTransferChange _fileDequeuedEvent;
        private event StartUpload _startUploadEvent;

        private string _uploadLocation;
        private Thread _uploadThread;
        private AutoResetEvent _reset;

        private bool _isRunning;

        private WebClient _wc;

        private static readonly object _lock = new object();

        public FileTransferManager(string UploadLocation)
        {
            _files = new ObservableCollection<FileTransfer>();
            _entryDispatcher = Dispatcher.CurrentDispatcher;
            _fileEnqueuedEvent += new FileTransferChange(OnFileEnqueued);
            _fileDequeuedEvent += new FileTransferChange(OnFileDequeued);
            _startUploadEvent += new StartUpload(StartNextUpload);

            _reset = new AutoResetEvent(false);

            _uploadLocation = UploadLocation;
            _uploadThread = new Thread(new ParameterizedThreadStart(run));

            _wc = new WebClient();
            _wc.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressChanged);
            _wc.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleted);
        }

        private void run(object param)
        {
            if (param == null)
                return;

            Dispatcher curDisp = (Dispatcher)param;

            _isRunning = true;
            try
            {
                while (_isRunning)
                {
                    curDisp.Invoke(DispatcherPriority.Normal, _startUploadEvent);
                    _reset.WaitOne();
                }
            }
            catch (ThreadAbortException tae)
            {
                _reset.Set();
            }
        }

        private void StartNextUpload()
        {
            _curTransfer = null;

            if (_files.Count < 1)
            {
                _isRunning = false;
                _reset.Set();
                return;
            }

            foreach (FileTransfer ft in _files)
            {
                if (ft.Progress < 100)
                {
                    _curTransfer = ft;
                    break;
                }
            }

            if (_curTransfer != null)
            {
                _curTransfer.Status = FileTransferStatus.InProgress;
                _wc.UploadFileAsync(new Uri(_uploadLocation), _curTransfer.FullPath);
            }
            else
            {
                Thread.Sleep(300);
            }
        }

        public void EnqueueFileTransfer(string fileName)
        {
            _entryDispatcher.Invoke(DispatcherPriority.Normal, _fileEnqueuedEvent, 
                new FileTransfer(new System.IO.FileInfo(fileName)));
        }

        public void DequeueFileTransfer(FileTransfer Transfer)
        {
            _entryDispatcher.Invoke(DispatcherPriority.Normal, _fileDequeuedEvent, Transfer);
        }

        public ObservableCollection<FileTransfer> Files
        {
            get { return _files; }
        }

        private void OnFileEnqueued(FileTransfer Transfer)
        {
            Transfer.Status = FileTransferStatus.Queued;
            _files.Add(Transfer);
            _uploadThread.Start(_entryDispatcher);
        }

        private void OnFileDequeued(FileTransfer Transfer)
        {
            if (Transfer.Status == FileTransferStatus.InProgress)
            {
                _wc.CancelAsync();
            }
            else
            {
                _files.Remove(Transfer);
            }
        }

        private void UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            _curTransfer.Progress = e.ProgressPercentage;
        }

        private void UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                _curTransfer.Status = FileTransferStatus.Completed;
            }
            else
            {
                _curTransfer.Status = FileTransferStatus.Cancelled;
            }
            _reset.Set();
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            _uploadThread.Abort();
            if (_wc.IsBusy)
            {
                _wc.CancelAsync();
            }
            _wc.Dispose();
        }

        #endregion
    }
}
