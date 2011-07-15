// Copyright (c) Manish Dalal, 2008-2009. All Rights Reserved.
// http://weblogs.asp.net/manishdalal
//
// This product's copyrights are licensed under 
// the CreativeCommons Attribution-ShareAlike (version 3)
// http://creativecommons.org/licenses/by-sa/3.0/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace SilverlightApplication.CustomSort {
    /// <summary>
    /// Implements ICollectionView to provide hook to plugin custom sorting
    /// </summary>
    /// <typeparam name="T">Type of Item in the collection</typeparam>
    public class SortableCollectionView<T> : ObservableCollection<T>, ICollectionView {

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableCollectionView&lt;T&gt;"/> class.
        /// </summary>
        public SortableCollectionView() {
            this._currentItem = null;
            this._currentPosition = -1;
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item) {
            //if (null != this.Filter && !this.Filter(item)) {
            //    return;
            //}
            base.InsertItem(index, item);
            if (0 == index || null == this._currentItem) {
                _currentItem = item;
                _currentPosition = index;
            }
        }

        /// <summary>
        /// Gets the item at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>item if found; otherwise, null</returns>
        public virtual object GetItemAt(int index) {
            if ((index >= 0) && (index < this.Count)) {
                return this[index];
            }
            return null;
        }

        #region ICollectionView Members

        /// <summary>
        /// Gets a value that indicates whether this view supports filtering by way of the <see cref="P:System.ComponentModel.ICollectionView.Filter"/> property.
        /// </summary>
        /// <value></value>
        /// <returns>true if this view supports filtering; otherwise, false.
        /// </returns>
        public bool CanFilter {
            get {
                //return true;
                return false;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether this view supports grouping by way of the <see cref="P:System.ComponentModel.ICollectionView.GroupDescriptions"/> property.
        /// </summary>
        /// <value></value>
        /// <returns>true if this view supports grouping; otherwise, false.
        /// </returns>
        public bool CanGroup {
            get { return false; }
        }

        /// <summary>
        /// Gets a value that indicates whether this view supports sorting by way of the <see cref="P:System.ComponentModel.ICollectionView.SortDescriptions"/> property.
        /// </summary>
        /// <value></value>
        /// <returns>true if this view supports sorting; otherwise, false.
        /// </returns>
        public bool CanSort {
            get { return true; }
        }

        /// <summary>
        /// Indicates whether the specified item belongs to this collection view.
        /// </summary>
        /// <param name="item">The object to check.</param>
        /// <returns>
        /// true if the item belongs to this collection view; otherwise, false.
        /// </returns>
        public bool Contains(object item) {
            if (!IsValidType(item)) {
                return false;
            }
            return this.Contains((T)item);
        }

        /// <summary>
        /// Determines whether the specified item is of valid type
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if the specified item is of valid type; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidType(object item) {
            return item is T;
        }

        private CultureInfo _culture;

        /// <summary>
        /// Gets or sets the cultural information for any operations of the view that may differ by culture, such as sorting.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The culture information to use during culture-sensitive operations.
        /// </returns>
        public System.Globalization.CultureInfo Culture {
            get {
                return this._culture;
            }
            set {
                if (this._culture != value) {
                    this._culture = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs("Culture"));
                }

            }
        }

        /// <summary>
        /// Occurs after the current item has been changed.
        /// </summary>
        public event EventHandler CurrentChanged;

        /// <summary>
        /// Occurs before the current item changes.
        /// </summary>
        public event CurrentChangingEventHandler CurrentChanging;

        private object _currentItem;

        /// <summary>
        /// Gets the current item in the view.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The current item in the view or null if there is no current item.
        /// </returns>
        public object CurrentItem {
            get { return this._currentItem; }
        }

        private int _currentPosition;

        /// <summary>
        /// Gets the ordinal position of the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> in the view.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The ordinal position of the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> in the view.
        /// </returns>
        public int CurrentPosition {
            get {
                return this._currentPosition;
            }
        }

        /// <summary>
        /// Enters a defer cycle that you can use to merge changes to the view and delay automatic refresh.
        /// </summary>
        /// <returns>
        /// The typical usage is to create a using scope with an implementation of this method 
        /// and then include multiple view-changing calls within the scope. 
        /// The implementation should delay automatic refresh until after the using scope exits.
        /// </returns>
        public IDisposable DeferRefresh() {
            return new DeferRefreshHelper(() => Refresh());
        }

        private Predicate<object> _filter;

        /// <summary>
        /// Gets or sets a callback that is used to determine whether an item is appropriate for inclusion in the view.
        /// </summary>
        /// <value></value>
        /// <returns>A method that is used to determine whether an item is appropriate for inclusion in the view.</returns>
        public Predicate<object> Filter {
            get {
                return _filter;
            }
            set {
                //if (value == _filter) return;
                _filter = value;
                //this.Refresh();
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="T:System.ComponentModel.GroupDescription"/> objects that describe how the items in the collection are grouped in the view.
        /// </summary>
        /// <value></value>
        /// <returns>A collection of objects that describe how the items in the collection are grouped in the view. </returns>
        public ObservableCollection<GroupDescription> GroupDescriptions {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the top-level groups.
        /// </summary>
        /// <value></value>
        /// <returns>A read-only collection of the top-level groups or null if there are no groups.</returns>
        public ReadOnlyObservableCollection<object> Groups {
            get {
                return null;//throw new NotImplementedException(); 
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> of the view is beyond the end of the collection.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> of the view is beyond the end of the collection; otherwise, false.
        /// </returns>
        public bool IsCurrentAfterLast {
            get {
                if (!this.IsEmpty) {
                    return (this.CurrentPosition >= this.Count);
                }
                return true;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> of the view is beyond the start of the collection.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> of the view is beyond the start of the collection; otherwise, false.
        /// </returns>
        public bool IsCurrentBeforeFirst {
            get {
                if (!this.IsEmpty) {
                    return (this.CurrentPosition < 0);
                }
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is current in sync.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is current in sync; otherwise, <c>false</c>.
        /// </value>
        protected bool IsCurrentInSync {
            get {
                if (this.IsCurrentInView) {
                    return (this.GetItemAt(this.CurrentPosition) == this.CurrentItem);
                }
                return (this.CurrentItem == null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is current in view.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is current in view; otherwise, <c>false</c>.
        /// </value>
        private bool IsCurrentInView {
            get {
                return ((0 <= this.CurrentPosition) && (this.CurrentPosition < this.Count));
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the view is empty.
        /// </summary>
        /// <value></value>
        /// <returns>true if the view is empty; otherwise, false.
        /// </returns>
        public bool IsEmpty {
            get {
                return (this.Count == 0);
            }
        }

        /// <summary>
        /// Sets the specified item in the view as the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/>.
        /// </summary>
        /// <param name="item">The item to set as the current item.</param>
        /// <returns>
        /// true if the resulting <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> is an item in the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentTo(object item) {
            if (!IsValidType(item)) {
                return false;
            }
            if (object.Equals(this.CurrentItem, item) && ((item != null) || this.IsCurrentInView)) {
                return this.IsCurrentInView;
            }
            int index = this.IndexOf((T)item);
            return this.MoveCurrentToPosition(index);
        }

        /// <summary>
        /// Sets the first item in the view as the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// true if the resulting <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> is an item in the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToFirst() {
            return this.MoveCurrentToPosition(0);
        }

        /// <summary>
        /// Sets the last item in the view as the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// true if the resulting <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> is an item in the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToLast() {
            return this.MoveCurrentToPosition(this.Count - 1);
        }

        /// <summary>
        /// Sets the item after the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> in the view as the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// true if the resulting <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> is an item in the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToNext() {
            return ((this.CurrentPosition < this.Count) && this.MoveCurrentToPosition(this.CurrentPosition + 1));
        }

        /// <summary>
        /// Sets the item before the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> in the view to the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// true if the resulting <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> is an item in the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToPrevious() {
            return ((this.CurrentPosition >= 0) && this.MoveCurrentToPosition(this.CurrentPosition - 1));
        }

        /// <summary>
        /// Sets the item at the specified index to be the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> in the view.
        /// </summary>
        /// <param name="position">The index to set the <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> to.</param>
        /// <returns>
        /// true if the resulting <see cref="P:System.ComponentModel.ICollectionView.CurrentItem"/> is an item in the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToPosition(int position) {
            if ((position < -1) || (position > this.Count)) {
                throw new ArgumentOutOfRangeException("position");
            }
            if (((position != this.CurrentPosition) || !this.IsCurrentInSync) && this.IsOKToChangeCurrent()) {
                bool isCurrentAfterLast = this.IsCurrentAfterLast;
                bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                ChangeCurrentToPosition(position);
                OnCurrentChanged();
                if (this.IsCurrentAfterLast != isCurrentAfterLast) {
                    this.OnPropertyChanged("IsCurrentAfterLast");
                }
                if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst) {
                    this.OnPropertyChanged("IsCurrentBeforeFirst");
                }
                this.OnPropertyChanged("CurrentPosition");
                this.OnPropertyChanged("CurrentItem");
            }
            return this.IsCurrentInView;
        }

        /// <summary>
        /// Changes the current to position.
        /// </summary>
        /// <param name="position">The position.</param>
        private void ChangeCurrentToPosition(int position) {
            if (position < 0) {
                this._currentItem = null;
                this._currentPosition = -1;
            } else if (position >= this.Count) {
                this._currentItem = null;
                this._currentPosition = this.Count;
            } else {
                this._currentItem = this[position];
                this._currentPosition = position;
            }
        }

        /// <summary>
        /// Determines whether it is OK to change current item.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if is OK to change current item; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsOKToChangeCurrent() {
            CurrentChangingEventArgs args = new CurrentChangingEventArgs();
            this.OnCurrentChanging(args);
            return !args.Cancel;
        }

        /// <summary>
        /// Called when current item has changed.
        /// </summary>
        protected virtual void OnCurrentChanged() {
            if (this.CurrentChanged != null) {
                this.CurrentChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CurrentChanging"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.ComponentModel.CurrentChangingEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCurrentChanging(CurrentChangingEventArgs args) {
            if (args == null) {
                throw new ArgumentNullException("args");
            }
            if (this.CurrentChanging != null) {
                this.CurrentChanging(this, args);
            }
        }

        /// <summary>
        /// Called when the current item is changing.
        /// </summary>
        protected void OnCurrentChanging() {
            this._currentPosition = -1;
            this.OnCurrentChanging(new CurrentChangingEventArgs(false));
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        protected override void ClearItems() {
            OnCurrentChanging();
            base.ClearItems();
        }

        /// <summary>
        /// Called when a property has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName) {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when data needs to be refreshed.
        /// </summary>
        public event EventHandler<RefreshEventArgs> OnRefresh;

        /// <summary>
        /// Recreates the view, by firing OnRefresh event.
        /// </summary>
        public void Refresh() {
            // sort and refersh
            if (null != OnRefresh) {
                OnRefresh(this, new RefreshEventArgs() { SortDescriptions = SortDescriptions });
            }
        }
        private SortDescriptionCollection _sortDescriptions;
        /// <summary>
        /// Gets a collection of <see cref="T:System.ComponentModel.SortDescription"/> instances that describe how the items in the collection are sorted in the view.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A collection of values that describe how the items in the collection are sorted in the view.
        /// </returns>
        public SortDescriptionCollection SortDescriptions {
            get {
                if (this._sortDescriptions == null) {
                    this._sortDescriptions = new SortDescriptionCollection();
                }

                return this._sortDescriptions;
            }
        }


        //private CustomSortDescriptionCollection _sort;

        //public SortDescriptionCollection SortDescriptions {
        //    get {
        //        if (this._sort == null) {
        //            this.SetSortDescriptions(new CustomSortDescriptionCollection());
        //        }
        //        return this._sort;
        //    }
        //}

        /// <summary>
        /// Sets the sort descriptions.
        /// </summary>
        /// <param name="descriptions">The descriptions.</param>
        //private void SetSortDescriptions(CustomSortDescriptionCollection descriptions) {
        //    if (this._sort != null) {
        //        this._sort.MyCollectionChanged -= new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
        //    }
        //    this._sort = descriptions;
        //    if (this._sort != null) {
        //        this._sort.MyCollectionChanged += new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
        //    }
        //}

        /// <summary>
        /// Sorts the descriptions changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        //private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e) {
        //    if (e.Action == NotifyCollectionChangedAction.Remove && e.NewStartingIndex == -1 && SortDescriptions.Count > 0) {
        //        return;
        //    }
        //    if (
        //        ((e.Action != NotifyCollectionChangedAction.Reset) || (e.NewItems != null))
        //        || (((e.NewStartingIndex != -1) || (e.OldItems != null)) || (e.OldStartingIndex != -1))
        //        ) {
        //        this.Refresh();
        //    }
        //}

        /// <summary>
        /// Gets the underlying collection.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The underlying collection.
        /// </returns>
        //public System.Collections.IEnumerable SourceCollection {
        //    get { 
        //        return this; 
        //    }
        //}

        public IEnumerable<T> SourceCollection {
            get {
                return this;
            }
            set {
                List<T> data = value.ToList<T>();
                this.Clear();
                foreach (T item in data) {
                    this.Add(item);
                }
            }
        }
        #endregion

        #region ICollectionView Members


        System.Collections.IEnumerable ICollectionView.SourceCollection {
            get { return this; }
        }

        #endregion

        #region Defer Refresh helper
        private class DeferRefreshHelper : IDisposable {
            private Action _callback;

            public DeferRefreshHelper(Action callback) {
                _callback = callback;
            }

            public void Dispose() {
                _callback();
            }
        }
        #endregion
    }
}
