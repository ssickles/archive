using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LoadingAdorner
{
    /// <summary>
    /// ThreadableObservableCollection caters for notify the user interface with changes done in the collection (Item[])
    /// This can be used when you have a multi threaded envirorment
    /// </summary>
    /// <typeparam name="T">The Type of objects that are going to be stored in this collection</typeparam>
    public class ThreadableObservableCollection<T> :
        System.Collections.ObjectModel.ObservableCollection<T>
    {
        private readonly List<T> items = null;

        private IComparer<T> comparer;

        /// <summary>
        /// Comparer used to sort the collection
        /// </summary>
        public IComparer<T> Comparer
        {
            get { return comparer; }
            set { comparer = value; }
        }

        private readonly T defaultLastValue;

        /// <summary>
        /// Gets the last value of the collection
        /// </summary>
        public T LastValue
        {
            get
            {
                if (items.Count != 0)
                    return items[Count > 0 ? (Count - 1) : 0];
                else
                    return defaultLastValue;
            }
        }


        /// <summary>
        /// controlDispatcher stored the dispatcher of the GUI control being daat bound
        /// </summary>
        private readonly Dispatcher controlDispatcher;

        /// <summary>
        /// ControlDispatcher returns the dispatcher of the GUI control being daat bound
        /// </summary>
        public Dispatcher ControlDispatcher
        {
            get { return controlDispatcher; }
        }

        /// <summary>
        /// Full constructor
        /// Sets the dispatcher for this view
        /// </summary>
        /// <param name="controlDispatcher">The dispatcher of the control being data bound</param>
        /// <param name="comparer">Comparer to sort the collection</param>
        /// <param name="defaultValue">The value to return from the LastValue property, if no data is present in the collection</param>
        public ThreadableObservableCollection(Dispatcher controlDispatcher, IComparer<T> comparer, T defaultValue)
            : this(controlDispatcher, comparer)
        {
            defaultLastValue = defaultValue;
        }

        /// <summary>
        /// Full constructor
        /// Sets the dispatcher for this view
        /// </summary>
        /// <param name="controlDispatcher">The dispatcher of the control being data bound</param>
        /// <param name="comparer">Comparer to sort the collection</param>
        public ThreadableObservableCollection(Dispatcher controlDispatcher, IComparer<T> comparer)
        {
            this.controlDispatcher = controlDispatcher;
            this.comparer = comparer;
            items = Items as List<T>;
        }

        //flag to signal if the collecion is being added with a chunk of data
        bool busy = false;

        /// <summary>
        /// flag to signal if the collecion is being added with a chunk of data
        /// </summary>
        public bool Busy
        {
            get { return busy; }
        }

        /// <summary>
        /// Add a collection to the list
        /// </summary>
        /// <param name="newItems">The collection of objects to add</param>
        public void AddRange(ICollection<T> newItems)
        {
            AddRange(newItems, true);
        }

        /// <summary>
        /// Add a collection to the list
        /// </summary>
        /// <param name="newItems">The collection of objects to add</param>
        /// <param name="resetAction">Pass true to raise the collection action reset event argument</param>
        public void AddRange(ICollection<T> newItems, bool resetAction)
        {
            //if only one item needs to be added call the add directly
            if (newItems.Count == 1)
            {
                IEnumerator<T> enumerator = newItems.GetEnumerator();
                enumerator.MoveNext();
                Add(enumerator.Current);
                return;
            }

            //set the collection as busy to turn OFF notifications
            busy = true;

            // if we have a comparer we might not add all items. put the ones we add in this list
            ICollection<T> addedItems = comparer == null ? newItems : new List<T>(newItems.Count);

            // add all the values in the collection
            foreach (T item in newItems)
            {
                int index = GetIndexForItem(item);
                // This check is done in InsertItem as well, but we need to know whether
                // insertion happened or not, to compose a proper list for the collectionchanged event
                if (comparer == null || index == 0 || Comparer.Compare(Items[index - 1], item) != 0)
                {
                    Items.Insert(index, item);
                    if (comparer != null)
                        addedItems.Add(item);
                }
            }

            //set the collection as not busy to turn ON notifications
            busy = false;

            if (resetAction)
            {
                //raise the collection changed event
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else
            {
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    (System.Collections.IList)addedItems));
            }
        }

        /// <summary>
        /// Removes a range of items from the list
        /// </summary>
        /// <param name="indexFrom">The index from where to begin removing</param>
        /// <param name="indexTo">The index from where to end removing</param>
        /// <param name="notifyUIOnce">Set to true if you want to only send one event to the UI when finished removing items</param>
        public void RemoveRange(int indexFrom, int indexTo, bool notifyUIOnce)
        {
            if (!notifyUIOnce)
            {
                RemoveRange(indexFrom, indexTo, this);
                return;
            }

            //set the collection as busy to turn OFF notifications
            busy = true;

            RemoveRange(indexFrom, indexTo, items);

            //set the collection as not busy to turn ON notifications
            busy = false;

            //raise the collection changed event
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset)
                );
        }

        /// <summary>
        /// Removes a range of items from the list
        /// </summary>
        /// <param name="indexFrom">The index from where to begin removing</param>
        /// <param name="indexTo">The index from where to end removing</param>
        /// <param name="collection">The collection to remove items from</param>
        private static void RemoveRange(int indexFrom, int indexTo, IList<T> collection)
        {
            for (int i = 0; i <= indexTo - indexFrom; i++)
            {
                if (collection.Count != 0)
                {
                    collection.RemoveAt(indexFrom);
                }
            }
        }

        /// <summary>
        /// raises the collection changed method
        /// </summary>
        /// <param name="e">The event argument to pass in the event</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!busy)
            {
                if (InvokeRequired)
                {
                    controlDispatcher.BeginInvoke(DispatcherPriority.DataBind, new OnCollectionChangedEventHandler(OnCollectionChanged), e);
                }
                else
                {
                    base.OnCollectionChanged(e);
                    base.OnPropertyChanged(new PropertyChangedEventArgs("LastValue"));
                }
            }
        }



        #region Delegates for callbacks

        /// <summary>
        /// InvokeRequired check if there need current thread is the main thread
        /// </summary>
        /// <returns>Returns true if the current thread is not the main thread</returns>
        private bool InvokeRequired
        {
            get
            {
                return controlDispatcher != null && controlDispatcher.Thread != System.Threading.Thread.CurrentThread;
            }
        }

        /// <summary>
        /// SetItemCallback is the delegate for when the SetItem method of the collection is invoked
        /// </summary>
        /// <param name="index">The index of the item</param>
        /// <param name="item">The new item data to set</param>
        private delegate void SetItemCallback(int index, T item);

        /// <summary>
        /// delegate to redirect to  the correct thread
        /// </summary>
        /// <param name="e">The event argument to pass in when the event is raised</param>
        private delegate void OnCollectionChangedEventHandler(NotifyCollectionChangedEventArgs e);

        /// <summary>
        /// RemoveItemCallback is the delegate for when an item is removed from the Collection
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        private delegate void RemoveItemCallback(int index);

        /// <summary>
        /// ClearItemsCallback is the delegate for when the clear method of the collection is called
        /// </summary>
        private delegate void ClearItemsCallback();

        /// <summary>
        /// InsertItemCallback is the delegate for when the InsertItem method of the collection is called
        /// </summary>
        /// <param name="index">The index where to insert the new item</param>
        /// <param name="item">The new item to add</param>
        private delegate void InsertItemCallback(int index, T item);

        #endregion

        #region Method To override

        /// <summary>
        /// InsertItem overrides the base InsertItem so to notify the GUI with the new data
        /// </summary>
        /// <param name="index">The index where the item was added</param>
        /// <param name="item">The item object added</param>
        protected override void InsertItem(int index, T item)
        {
            if (index == 0 || comparer == null || Comparer.Compare(Items[index - 1], item) != 0)
            {
                if (InvokeRequired)
                    controlDispatcher.Invoke(DispatcherPriority.Send, new InsertItemCallback(InsertItem), index, new object[] { item });
                else
                    base.InsertItem(GetIndexForItem(item), item);
            }
        }

        /// <summary>
        /// Gets the index of where to insert the item in the list.
        /// </summary>
        /// <param name="item">The Item to search</param>
        /// <returns>Return the index of the item</returns>
        public int GetIndexForItem(T item)
        {
            if (items.Count == 0)
                return 0;

            //Compare with the last item.
            if (comparer == null ||
                Comparer.Compare(items[items.Count - 1], item) <= 0)
                return items.Count;

            int index = items.BinarySearch(item, comparer);

            // Item was found. Insert new item after
            if (index >= 0)
                index++;

            // Item was not found. Bitwise complement is where to put the new one.
            if (index < 0)
                index = ~index;

            return index;
        }

        /// <summary>
        /// SetItem modifies an item in the collection
        /// </summary>
        /// <param name="index">The index of the item to modify</param>
        /// <param name="item">The new item instance</param>
        protected override void SetItem(int index, T item)
        {
            if (InvokeRequired)
                controlDispatcher.Invoke(DispatcherPriority.DataBind, new SetItemCallback(SetItem), index, new object[] { item });
            else
                base.SetItem(index, item);
        }

        /// <summary>
        /// RemoveItem removes an item from a specific position
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        protected override void RemoveItem(int index)
        {
            if (InvokeRequired)
                controlDispatcher.Invoke(DispatcherPriority.DataBind, new RemoveItemCallback(RemoveItem), index);
            else
                base.RemoveItem(index);
        }

        /// <summary>
        /// ClearItems will remove all the items from the collection
        /// </summary>
        protected override void ClearItems()
        {
            if (InvokeRequired)
                ControlDispatcher.Invoke(DispatcherPriority.DataBind, new ClearItemsCallback(ClearItems));
            else
                base.ClearItems();
        }

        #endregion
    }
}
