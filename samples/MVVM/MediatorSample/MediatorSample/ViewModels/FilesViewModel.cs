using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMHelper.ViewModel;
using MVVMHelper.MediatorLib;
using FileSystemServices;
using System.IO;

namespace MediatorSample.ViewModels
{
    public class FilesViewModel : BaseViewModel
    {
        #region Properties
        public bool HasItemsInDataSource
        {
            get { return !IsDataSourceEmpty; }
        }

        /// <summary>
        /// Gets a bool that states if the source has children
        /// </summary>
        public bool IsDataSourceEmpty
        {
            get
            {
                if (DataSource == null)
                    return true;
                return DataSource.Count == 0;
            }
        }

        IList<FileInfo> dataSource;
        /// <summary>
        /// Gets or sets the DataSource that contians the list of files
        /// </summary>
        public IList<FileInfo> DataSource
        {
            get { return dataSource; }
            set 
            {
                dataSource = value;
                RaisePropertyChanged("DataSource");
                RaisePropertyChanged("IsDataSourceEmpty");
                RaisePropertyChanged("HasItemsInDataSource");
            }
        }
        #endregion

        public void Initialize()
        {
            GetService<Mediator>().Register(Messages.DirectorySelectedChanged,
                (Action<DirectoryDisplayItem>)OnDirectorySelected);
        }

        #region IColleague interface
        void OnDirectorySelected(DirectoryDisplayItem selectedItem)
        {
            //load all files for the directory specified
            DataSource = GetService<IExplorer>().GetChildFiles(selectedItem.Path);
        }
        #endregion
    }
}
