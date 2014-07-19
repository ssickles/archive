using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WickedSick.CommonComponents.IO
{
    public class FileChange
    {
        private string _filename = string.Empty;
        private WatcherChangeTypes _change;
        private DateTime _time;

        public FileChange()
        {

        }

        public FileChange(string Filename, WatcherChangeTypes Change, DateTime Time)
        {
            _filename = Filename;
            _change = Change;
            _time = Time;
        }

        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public WatcherChangeTypes Change
        {
            get { return _change; }
            set { _change = value; }
        }

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
    }
}
