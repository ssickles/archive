using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonComponents.Sync.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            SyncManager _syncManager = new SyncManager(1000);

            _syncManager.AddSync(new QueueCollection());
            _syncManager.SyncBegin += new SyncManager.SyncBeginHandler(_syncManager_SyncBegin);
            _syncManager.SyncComplete += new EventHandler(_syncManager_SyncComplete);
            _syncManager.SyncFailed += new SyncManager.SyncFailedHandler(_syncManager_SyncFailed);
            _syncManager.SyncRetry += new SyncManager.SyncRetryHandler(_syncManager_SyncRetry);
            _syncManager.SyncFinish += new SyncManager.SyncFinishHandler(_syncManager_SyncFinish);

            _syncManager.BeginSync();

            Console.ReadLine();

            _syncManager.EndSync();
        }

        static void _syncManager_SyncComplete(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("Sync Complete"));
        }

        static void _syncManager_SyncFinish(object sender, SyncEventArgs SyncArgs)
        {
            Console.WriteLine(string.Format("Sync Finish: {0}", SyncArgs.Sync.Name));
        }

        static void _syncManager_SyncRetry(object sender, SyncEventArgs SyncArgs)
        {
            Console.WriteLine(string.Format("Sync Retry: {0}", SyncArgs.Sync.Name));
        }

        static void _syncManager_SyncFailed(object sender, SyncEventArgs SyncArgs)
        {
            Console.WriteLine(string.Format("Sync Failed: {0}", SyncArgs.Sync.Name));
        }

        static void _syncManager_SyncBegin(object sender, SyncEventArgs SyncArgs)
        {
            Console.WriteLine(string.Format("Sync Begin: {0}", SyncArgs.Sync.Name));
        }
    }
}
