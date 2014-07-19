using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Net;

namespace WickedSick.CommonComponents.IO
{
    public class FileTransferManager2: IDisposable
    {
        private ObservableCollection<FileTransfer> _transfers;
        private int _maxTransfers = 5;
        private int _currentTransfers = 0;
        object _locker = new object();
        //FileTransferQueue _queue;

        public FileTransferManager2(int MaxConcurrentTransfers)
        {
            _maxTransfers = MaxConcurrentTransfers;
            _transfers = new ObservableCollection<FileTransfer>();
            //_queue = new FileTransferQueue();
        }

        public IList<FileTransfer> Transfers
        {
            get { return _transfers; }
        }

        public int MaxConcurrentTransfers
        {
            get { return _maxTransfers; }
            set { _maxTransfers = value; }
        }

        public void AddTransfer(FileTransfer Transfer)
        {
            _transfers.Add(Transfer);
            Transfer.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Transfer_PropertyChanged);
            StartTransfer();
        }

        private void Transfer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            FileTransfer transfer = (FileTransfer)sender;
            if (e.PropertyName.Equals("Status"))
            {
                if (transfer.Status == FileTransferStatus.Completed || transfer.Status == FileTransferStatus.Cancelled)
                {
                    lock (_locker)
                    {
                        _currentTransfers--;
                    }
                    StartTransfer();
                }

            }
        }

        private void StartTransfer()
        {
            if (_currentTransfers < _maxTransfers)
            {
                foreach (FileTransfer transfer in _transfers)
                {
                    if (transfer.Status == FileTransferStatus.NotStarted)
                    {
                        lock (_locker)
                        {
                            _currentTransfers++;
                        }
                        //_queue.EnqueueTransfer(transfer);
                        transfer.Status = FileTransferStatus.Queued;
                        transfer.Start();
                        break;
                    }
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            _transfers.Clear();
        }

        #endregion
    }
}
