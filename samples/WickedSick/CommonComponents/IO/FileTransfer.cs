using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace WickedSick.CommonComponents.IO
{
    public enum FileTransferStatus
    {
        NotStarted,
        Queued,
        InProgress,
        Completed,
        Cancelled
    }

    public class FileTransfer : INotifyPropertyChanged, IDisposable
    {
        private string _destination = "";
        private int _progress = 0;
        private FileTransferStatus _status = FileTransferStatus.NotStarted;
        private FileInfo _file;
        private WebClient _wc;

        public FileTransfer(FileInfo file)
        {
            _file = file;
            _wc = new WebClient();
            _wc.UploadFileCompleted += new UploadFileCompletedEventHandler(_wc_UploadFileCompleted);
            _wc.UploadProgressChanged += new UploadProgressChangedEventHandler(_wc_UploadProgressChanged);
        }

        void _wc_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            _progress = (int)(e.BytesSent / e.TotalBytesToSend);
            _status = FileTransferStatus.InProgress;
        }

        void _wc_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            _progress = 100;
            _status = FileTransferStatus.Completed;
        }

        #region PROPERTIES

        public string FullPath
        {
            get { return _file.FullName; }
        }

        public string Filename
        {
            get 
            { 
                return ((_file != null) ? _file.Name : "");
            }
        }

        public string Destination
        {
            get { return _destination; }
            set 
            { 
                _destination = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Destination"));
            }
        }

        public int Progress
        {
            get { return _progress; }
            set 
            { 
                _progress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Progress"));
            }
        }

        public FileTransferStatus Status
        {
            get { return _status; }
            set 
            { 
                _status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        #endregion

        #region METHODS
        public void Start()
        {

            _wc.UploadFileAsync(new Uri(_destination), _file.FullName);
        }

        public void Pause()
        {

        }

        public void Continue()
        {

        }

        public void Cancel()
        {
            _wc.CancelAsync();
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _wc.Dispose();
        }

        #endregion
    }
}
