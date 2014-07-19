using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMHelper.ViewModel;
using System.Collections.ObjectModel;
using FileSystemServices;
using System.IO;
using MVVMHelper.MediatorLib;
using System.Windows.Input;
using MVVMHelper.Commands;

namespace MediatorSample.ViewModels
{
    public class DirectoriesViewModel : BaseViewModel
    {
        #region Properties

        ICommand getChildren;

        /// <summary>
        /// Gets a command that should be executed when an item is expanded
        /// </summary>
        public ICommand GetChildren
        {
            get { return getChildren; }
        }

        IList<DirectoryDisplayItem> dataSource;
        /// <summary>
        /// Gets the list of items to display
        /// </summary>
        public IList<DirectoryDisplayItem> DataSource
        {
            get { return dataSource; }
            private set
            {
                dataSource = value;
                RaisePropertyChanged("DataSource");
            }
        }

        DirectoryDisplayItem selectedItem;  

        /// <summary>
        /// Gets or sets the selected item
        /// </summary>
        public DirectoryDisplayItem SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
                //Send a message that an item is selected and pass the object selected
                GetService<Mediator>().NotifyColleagues(
                    Messages.DirectorySelectedChanged, SelectedItem);
            }
        }
         
         #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public DirectoriesViewModel()
        {
            getChildren = new RelayCommand(GetChildrenForItem);
        }

        /// <summary>
        /// Loads the initial data set
        /// </summary>
        public void LoadData()
        {
            var service = GetService<IExplorer>();
            DataSource = (from x in service.GetRootDirectories()
                          select new DirectoryDisplayItem { Name = x.Name, Path = x.FullName }).ToList();
        }

        
        void GetChildrenForItem(object itemExpanded)
        {
            //get the object being expanded
            DirectoryDisplayItem displayItem = (DirectoryDisplayItem)itemExpanded;

            //check if the children of this item are dummy items
            if (displayItem.ContainsDummyItems)
            {
                //if the children are dummy children replace them with the actual data
                displayItem.ChildItems.Clear();
                var modelData = GetService<IExplorer>().GetChildDirectories(displayItem.Path);
                if (modelData != null)
                {
                    var children = from x in modelData
                                   select new DirectoryDisplayItem { Name = x.Name, Path = x.FullName };

                    //add all the data to the list
                    foreach (var child in children)
                        displayItem.ChildItems.Add(child);
                    displayItem.ContainsDummyItems = false;
                }
            }
        }
    }
}
