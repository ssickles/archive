using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using IdentityStream.Presentation.Mvvm;

namespace ActivityControlTest
{
    public class Window1ViewModel: INotifyPropertyChanged
    {
        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            private set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
        }

        private List<string> _people;
        public List<string> People
        {
            get
            {
                return _people;
            }
            private set
            {
                if (_people != value)
                {
                    _people = value;
                    OnPropertyChanged("People");
                }
            }
        }

        private ICommand _reloadCommand;
        public ICommand ReloadCommand
        {
            get
            {
                if (_reloadCommand == null)
                {
                    _reloadCommand = new RelayCommand(
                        e => { Reload(); },
                        e => { return !IsActive; }
                        );
                }
                return _reloadCommand;
            }
        }
        private void Reload()
        {
            _loadPeopleWorker.RunWorkerAsync();
        }

        private BackgroundWorker _loadPeopleWorker;
        public Window1ViewModel()
        {
            _loadPeopleWorker = new BackgroundWorker();
            _loadPeopleWorker.DoWork += new DoWorkEventHandler(_loadPeopleWorker_DoWork);
            _loadPeopleWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_loadPeopleWorker_RunWorkerCompleted);
            _loadPeopleWorker.RunWorkerAsync();
        }

        private void _loadPeopleWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            People = new List<string>() {
                "Scott Sickles",
                "Charlotte Sickles",
                "Connor Sickles",
                "Brad Sickles",
                "Tracy Sickles",
                "Ryan Holtzer",
                "Kristen Lewis",
                "David Lewis",
                "Carol Sickles",
                "Dale Sickles"
            };
            IsActive = false;
        }

        private void _loadPeopleWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IsActive = true;
            Thread.Sleep(5000);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler obj = PropertyChanged;
            if (obj != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
