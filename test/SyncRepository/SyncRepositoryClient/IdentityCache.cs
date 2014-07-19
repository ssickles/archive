using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SyncRepositoryDomainModel;
using System.Diagnostics;

namespace SyncRepositoryClient
{
    public static class IdentityCache
    {
        private static DateTime _lastUpdated = DateTime.MinValue;
        private static BackgroundWorker _syncWorker;
        private static object locker = new object();

        static IdentityCache()
        {
            if (Identities == null)
                Identities = new ObservableCollection<Identity>();

            _syncWorker = new BackgroundWorker();
            _syncWorker.DoWork += new DoWorkEventHandler(_syncWorker_DoWork);
            _syncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_syncWorker_RunWorkerCompleted);
        }

        public static ObservableCollection<Identity> Identities { get; set; }

        private static void _syncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void _syncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClient syncProxy = new SyncClient();
            List<Identity> updates = syncProxy.GetIdentities(new Request() { LastRequest = _lastUpdated });
            lock (locker)
            {
                foreach (Identity upd in updates)
                {
                    Identity id = Identities.FirstOrDefault<Identity>(delegate(Identity Iden) { return Iden.Id == upd.Id; });
                    if (id == null)
                    {
                        Identities.Add(upd);
                    }
                    else
                    {
                        id.FirstName = upd.FirstName;
                        id.LastName = upd.LastName;
                        id.LastUpdated = upd.LastUpdated;
                    }
                }
                _lastUpdated = DateTime.Now;
            }
        }

        public static void StartSync()
        {
            _syncWorker.RunWorkerAsync();
        }
    }
}
