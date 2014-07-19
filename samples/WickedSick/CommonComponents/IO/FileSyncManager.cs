using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace WickedSick.CommonComponents.IO
{
    public class FileSyncManager
    {
        private ObservableCollection<FileSyncLocation> _locations;

        public FileSyncManager()
        {
            _locations = new ObservableCollection<FileSyncLocation>();
        }

        public IList<FileSyncLocation> Locations
        {
            get { return _locations; }
        }

        public void AddLocation(string Path)
        {
            _locations.Add(new FileSyncLocation(Path));
        }
    }
}
