using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace CommonComponents.Sync
{
    public class SyncManager: IDisposable
    {
        public delegate void SyncBeginHandler(object sender, SyncEventArgs SyncArgs);
        public delegate void SyncRetryHandler(object sender, SyncEventArgs SyncArgs);
        public delegate void SyncFailedHandler(object sender, SyncEventArgs SyncArgs);
        public delegate void SyncFinishHandler(object sender, SyncEventArgs SyncArgs);

        public event SyncBeginHandler SyncBegin;
        public event EventHandler SyncComplete;
        public event SyncRetryHandler SyncRetry;
        public event SyncFailedHandler SyncFailed;
        public event SyncFinishHandler SyncFinish;

        private System.Timers.Timer _timer;
        private int _syncFrequency;
        private int _runningSyncs = 0;
        private Dictionary<string, ISync> _syncs = new Dictionary<string, ISync>();
        private List<BackgroundWorker> _workers = new List<BackgroundWorker>();
        private object _locker = new object();

        public SyncManager(int Frequency)
        {
            _syncFrequency = Frequency;
            //the timer is used to signal the beginning of a synchronization
            _timer = new System.Timers.Timer(_syncFrequency);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
        }

        public int SyncFrequency
        {
            get { return _syncFrequency; }
            set { _syncFrequency = value; }
        }

        public void AddSync(ISync Sync)
        {
            if (_syncs.ContainsKey(Sync.Name))
            {
                throw new Exception("Sync Name already exists in the SyncManager.");
            }
            else
            {
                lock(_locker)
                {
                    _syncs.Add(Sync.Name, Sync);
                }
            }
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //execute the synchronizations, but only if the previous syncs are finished
            if (_runningSyncs == 0)
            {
                Run();
            }
        }

        public void BeginSync()
        {
            _timer.Start();
        }

        public void EndSync()
        {
            _timer.Stop();
        }

        private void Run()
        {
            //lock access to the _syncs so that none can be added while performing this synchronization
            lock (_locker)
            {
                foreach (ISync sync in _syncs.Values)
                {
                    //use a background worker to execute each ISync on a different thread
                    BackgroundWorker newWorker = new BackgroundWorker();
                    _workers.Add(newWorker);
                    _runningSyncs++;
                    //the DoWork event establishes the method to be executed on the background worker
                    newWorker.DoWork += new DoWorkEventHandler(newWorker_DoWork);
                    newWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(newWorker_RunWorkerCompleted);
                    //execute the background process
                    //the sync object is passed as the argument so that we know which sync to execute
                    newWorker.RunWorkerAsync(sync);
                }
            }
        }

        private void newWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //get the sync out of the argument and execute it
            ISync sync = (ISync)e.Argument;
            OnSyncBegin(sync);
            e.Result = sync.Run();
        }

        private void newWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //after each sync is finished, events are fired, cleanup is done, etc
            _workers.Remove((BackgroundWorker)sender);
            _runningSyncs--;

            if (_runningSyncs == 0)
            {
                OnSyncComplete();
            }

            if ((SyncResult)e.Result == SyncResult.Success)
            {
                //OnSyncFinish(, (SyncResult)e.Result);
            }
            else
            {
                //the sync failed so do something different here
                OnSyncFailed((ISync)sender);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            EndSync();
            _syncs.Clear();
            _workers.Clear();
        }

        #endregion

        private void OnSyncFinish(ISync Sync, SyncResult SyncResult)
        {
            SyncFinishHandler _eventObj = SyncFinish;
            if (_eventObj != null)
            {
                _eventObj(this, new SyncFinishEventArgs(Sync, SyncResult));
            }
        }

        private void OnSyncBegin(ISync Sync)
        {
            SyncBeginHandler _eventObj = SyncBegin;
            if (_eventObj != null)
            {
                _eventObj(this, new SyncEventArgs(Sync));
            }
        }

        private void OnSyncComplete()
        {
            EventHandler _eventObj = SyncComplete;
            if (_eventObj != null)
            {
                _eventObj(this, new EventArgs());
            }
        }

        private void OnSyncRetry(ISync Sync)
        {
            SyncRetryHandler _eventObj = SyncRetry;
            if (_eventObj != null)
            {
                _eventObj(this, new SyncEventArgs(Sync));
            }
        }

        private void OnSyncFailed(ISync Sync)
        {
            SyncFailedHandler _eventObj = SyncFailed;
            if (_eventObj != null)
            {
                _eventObj(this, new SyncEventArgs(Sync));
            }
        }
    }

    public class SyncEventArgs : EventArgs
    {
        private ISync _sync;

        public SyncEventArgs(ISync Sync)
        {
            _sync = Sync;
        }

        public ISync Sync
        {
            get { return _sync; }
        }
    }

    public class SyncFinishEventArgs : SyncEventArgs
    {
        private SyncResult _syncResult;

        public SyncFinishEventArgs(ISync Sync, SyncResult SyncResult): base(Sync)
        {
            _syncResult = SyncResult;
        }

        public SyncResult Result
        {
            get { return _syncResult; }
        }
    }
}
