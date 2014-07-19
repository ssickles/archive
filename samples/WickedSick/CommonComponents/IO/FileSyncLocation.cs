using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Threading;
using System.Threading;

namespace WickedSick.CommonComponents.IO
{
    delegate void AddChangeDelegate(FileChange Change);

    public class FileSyncLocation: IDisposable
    {
        private FileSystemWatcher _watcher;
        private ObservableCollection<FileChange> _changes;
        private Dispatcher _entryDispatcher;
        private event AddChangeDelegate _addChangeEvent;

        public FileSyncLocation(string Path)
        {
            _changes = new ObservableCollection<FileChange>();
            SetupWatcher(Path);
            _entryDispatcher = Dispatcher.CurrentDispatcher;
            _addChangeEvent += new AddChangeDelegate(AddChange);
        }

        public string Path
        {
            get { return _watcher.Path; }
        }

        public IList<FileChange> Changes
        {
            get { return _changes; }
        }

        private void SetupWatcher(string Path)
        {
            _watcher = new FileSystemWatcher(Path);

            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                NotifyFilters.DirectoryName | NotifyFilters.CreationTime | NotifyFilters.Attributes |
                NotifyFilters.Size;

            _watcher.Renamed += new RenamedEventHandler(watcher_Renamed);
            _watcher.Deleted += new FileSystemEventHandler(watcher_Changed);
            _watcher.Created += new FileSystemEventHandler(watcher_Changed);
            _watcher.Changed += new FileSystemEventHandler(watcher_Changed);

            _watcher.IncludeSubdirectories = true;

            _watcher.EnableRaisingEvents = true;
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            FileChange change = new FileChange();
            change.Filename = e.FullPath;
            change.Change = e.ChangeType;
            change.Time = DateTime.Now;

            //_changes.Add(change);
            _entryDispatcher.Invoke(DispatcherPriority.DataBind, _addChangeEvent, change);
        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            FileChange change1 = new FileChange();
            change1.Filename = e.OldFullPath;
            change1.Change = e.ChangeType;
            change1.Time = DateTime.Now;

            //_changes.Add(change1);
            _entryDispatcher.Invoke(DispatcherPriority.DataBind, _addChangeEvent, change1);

            FileChange change2 = new FileChange();
            change2.Filename = e.FullPath;
            change2.Change = e.ChangeType;
            change2.Time = DateTime.Now;

            //_changes.Add(change2);
            _entryDispatcher.Invoke(DispatcherPriority.DataBind, _addChangeEvent, change2);
        }

        private void AddChange(FileChange Change)
        {
            _changes.Add(Change);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _watcher.Dispose();
            _changes = null;
        }

        #endregion
    }
}
