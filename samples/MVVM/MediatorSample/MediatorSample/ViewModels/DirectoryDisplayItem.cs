using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MediatorSample.ViewModels
{
    /// <summary>
    /// Object to represent the directory structure
    /// This class adds a dummy item to the children so that we can have lazy loading of data for the treeview
    /// </summary>
    public class DirectoryDisplayItem
    {
        const string DummyItem = "Dummy";

        #region properties
        /// <summary>
        /// Gets or sets a flag to mark this object as having dummy items as children
        /// </summary>
        public bool ContainsDummyItems { get; set; }

        /// <summary>
        /// Gets the name of the Directory
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the path of the Directory
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets the list of child items
        /// This needs to be an observable collection so we can modify the content and keep the UI updated
        /// The collection will be modified as soom as the treeview is expanded and only dummy items are found
        /// </summary>
        public ObservableCollection<DirectoryDisplayItem> ChildItems
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public DirectoryDisplayItem()
        {
            //only do this operation if the 
            ChildItems = new ObservableCollection<DirectoryDisplayItem>();
            //Add a Dummy item by default so that the + sign appears in the treeview
            ChildItems.Add(CreateDummy());
            ContainsDummyItems = true;
        }

        /// <summary>
        /// Constructor to create a dummy element
        /// </summary>
        /// <param name="dummyElementName">The name of the dummy element </param>
        private DirectoryDisplayItem(string dummyElementName)
        { }

        #region helper methods
        /// <summary>
        /// Checks if the specified item is a dummy item
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>Returns true if the item is a dummy item</returns>
        public static bool IsDummyItem(DirectoryDisplayItem item)
        {
            return item.Name == DummyItem;
        }

        /// <summary>
        /// Creates a dummy element to be used for the child items
        /// </summary>
        /// <returns>Returns a dummy element</returns>
        private static DirectoryDisplayItem CreateDummy()
        {
            return new DirectoryDisplayItem(DummyItem);
        }
        #endregion
    }
}
