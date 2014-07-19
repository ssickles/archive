using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;

namespace IdentityStream.Windows.Controls
{
    public sealed class PagedCollectionView : ICollectionView, IEnumerable, INotifyCollectionChanged, IPagedCollectionView, IEditableCollectionView, INotifyPropertyChanged
    {
        // Fields
        private int _cachedPageIndex;
        private int _cachedPageSize;
        private CultureInfo _culture;
        private SimpleMonitor _currentChangedMonitor;
        private object _currentItem;
        private int _currentPosition;
        private int _deferLevel;
        private object _editItem;
        private Predicate<object> _filter;
        private CollectionViewFlags _flags;
        private CollectionViewGroupRoot _group;
        private IList _internalList;
        private bool _isGrouping;
        private bool _isUsingTemporaryGroup;
        private ConstructorInfo _itemConstructor;
        private bool _itemConstructorIsValid;
        private object _newItem;
        private int _pageIndex;
        private int _pageSize;
        private bool _pollForChanges;
        private SortDescriptionCollection _sortDescriptions;
        private IEnumerable _sourceCollection;
        private CollectionViewGroupRoot _temporaryGroup;
        private int _timestamp;
        private IEnumerator _trackingEnumerator;
        private static readonly CurrentChangingEventArgs uncancelableCurrentChangingEventArgs = new CurrentChangingEventArgs(false);

        // Events
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler CurrentChanged;

        public event CurrentChangingEventHandler CurrentChanging;

        public event EventHandler<EventArgs> PageChanged;

        public event EventHandler<PageChangingEventArgs> PageChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        event NotifyCollectionChangedEventHandler CollectionChanged;

        event PropertyChangedEventHandler PropertyChanged;

        // Methods
        public PagedCollectionView(IEnumerable source) : this(source, false, false)
        {
        }

        public PagedCollectionView(IEnumerable source, bool isDataSorted, bool isDataInGroupOrder)
        {
            NotifyCollectionChangedEventHandler handler = null;
            this._cachedPageIndex = -1;
            this._currentChangedMonitor = new SimpleMonitor();
            this._flags = CollectionViewFlags.ShouldProcessCollectionChanged;
            this._pageIndex = -1;
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            this._sourceCollection = source;
            this.SetFlag(CollectionViewFlags.IsDataSorted, isDataSorted);
            this.SetFlag(CollectionViewFlags.IsDataInGroupOrder, isDataInGroupOrder);
            this._temporaryGroup = new CollectionViewGroupRoot(this, isDataInGroupOrder);
            this._group = new CollectionViewGroupRoot(this, false);
            this._group.GroupDescriptionChanged += new EventHandler(this.OnGroupDescriptionChanged);
            this._group.GroupDescriptions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnGroupByChanged);
            this.CopySourceToInternalList();
            this._trackingEnumerator = source.GetEnumerator();
            if (this._internalList.Count > 0)
            {
                this.SetCurrent(this._internalList[0], 0, 1);
            }
            else
            {
                this.SetCurrent(null, -1, 0);
            }
            this.SetFlag(CollectionViewFlags.CachedIsEmpty, this.Count == 0);
            this._pollForChanges = !(source is INotifyCollectionChanged);
            if (!this._pollForChanges)
            {
                if (handler == null)
                {
                    handler = delegate (object sender, NotifyCollectionChangedEventArgs args) {
                        this.ProcessCollectionChanged(args);
                    };
                }
                (source as INotifyCollectionChanged).CollectionChanged += handler;
            }
        }

        public object AddNew()
        {
            int count;
            this.EnsureCollectionInSync();
            this.VerifyRefreshNotDeferred();
            if (this.IsEditingItem)
            {
                this.CommitEdit();
            }
            this.CommitNew();
            if (!this.CanAddNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedForView, new object[] { "AddNew" }));
            }
            object obj2 = null;
            if (this._itemConstructor != null)
            {
                obj2 = this._itemConstructor.Invoke(null);
            }
            try
            {
                this.SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, false);
                if (this.SourceList != null)
                {
                    this.SourceList.Add(obj2);
                }
            }
            finally
            {
                this.SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, true);
            }
            this._trackingEnumerator = this._sourceCollection.GetEnumerator();
            int index = -1;
            if (this.PageSize > 0)
            {
                count = this.Count - ((this.Count == this.PageSize) ? 1 : 0);
                index = (this.Count == this.PageSize) ? count : -1;
            }
            else
            {
                count = this.Count;
            }
            if (index > -1)
            {
                object itemAt = this.GetItemAt(index);
                if (this.IsGrouping)
                {
                    this._group.RemoveFromSubgroups(itemAt);
                }
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, itemAt, index));
            }
            this._internalList.Insert(this.ConvertToInternalIndex(count), obj2);
            this.OnPropertyChanged("ItemCount");
            object currentItem = this.CurrentItem;
            int currentPosition = this.CurrentPosition;
            bool isCurrentAfterLast = this.IsCurrentAfterLast;
            bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
            this.AdjustCurrencyForAdd(null, count);
            if (this.IsGrouping)
            {
                this._group.InsertSpecialItem(this._group.Items.Count, obj2, false);
                if (this.PageSize > 0)
                {
                    this._temporaryGroup.InsertSpecialItem(this._temporaryGroup.Items.Count, obj2, false);
                }
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, obj2, count));
            this.RaiseCurrencyChanges(false, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
            this.CurrentAddItem = obj2;
            this.MoveCurrentTo(obj2);
            IEditableObject obj5 = obj2 as IEditableObject;
            if (obj5 != null)
            {
                obj5.BeginEdit();
            }
            return obj2;
        }

        private void AdjustCurrencyForAdd(object newCurrentItem, int index)
        {
            if (newCurrentItem != null)
            {
                int newPosition = this.IndexOf(newCurrentItem);
                if ((newPosition >= 0) && ((newPosition != this.CurrentPosition) || !this.IsCurrentInSync))
                {
                    this.OnCurrentChanging();
                    this.SetCurrent(newCurrentItem, newPosition);
                }
            }
            else if (this.Count == 1)
            {
                if ((this.CurrentItem != null) || (this.CurrentPosition != -1))
                {
                    this.OnCurrentChanging();
                }
                this.SetCurrent(null, -1);
            }
            else if (index <= this.CurrentPosition)
            {
                this.OnCurrentChanging();
                int num2 = this.CurrentPosition + 1;
                if (num2 >= this.Count)
                {
                    num2 = this.Count - 1;
                }
                this.SetCurrent(this.GetItemAt(num2), num2);
            }
        }

        private void AdjustCurrencyForEdit(object newCurrentItem, int index)
        {
            if ((newCurrentItem != null) && (this.IndexOf(newCurrentItem) >= 0))
            {
                this.OnCurrentChanging();
                this.SetCurrent(newCurrentItem, this.IndexOf(newCurrentItem));
            }
            else if (index <= this.CurrentPosition)
            {
                this.OnCurrentChanging();
                int newPosition = this.CurrentPosition + 1;
                if (newPosition < this.Count)
                {
                    this.SetCurrent(this.GetItemAt(newPosition), newPosition);
                }
                else
                {
                    this.SetCurrent(null, this.Count);
                }
            }
        }

        private void AdjustCurrencyForRemove(int index)
        {
            if (index < this.CurrentPosition)
            {
                this.OnCurrentChanging();
                this.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
            }
            if (this.CurrentPosition >= this.Count)
            {
                this.OnCurrentChanging();
                this.SetCurrentToPosition(this.Count - 1);
            }
            if (!this.IsCurrentInSync)
            {
                this.OnCurrentChanging();
                this.SetCurrentToPosition(this.CurrentPosition);
            }
        }

        public void CancelEdit()
        {
            if (this.IsAddingNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringTransaction, new object[] { "CancelEdit", "AddNew" }));
            }
            if (!this.CanCancelEdit)
            {
                throw new InvalidOperationException(PagedCollectionViewResources.CancelEditNotSupported);
            }
            this.VerifyRefreshNotDeferred();
            if (this.CurrentEditItem != null)
            {
                object currentEditItem = this.CurrentEditItem;
                this.CurrentEditItem = null;
                IEditableObject obj3 = currentEditItem as IEditableObject;
                if (obj3 == null)
                {
                    throw new InvalidOperationException(PagedCollectionViewResources.CancelEditNotSupported);
                }
                obj3.CancelEdit();
            }
        }

        public void CancelNew()
        {
            if (this.IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringTransaction, new object[] { "CancelNew", "EditItem" }));
            }
            this.VerifyRefreshNotDeferred();
            if (this.CurrentAddItem != null)
            {
                int index = this.IndexOf(this.CurrentAddItem);
                try
                {
                    this.SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, false);
                    if (this.SourceList != null)
                    {
                        this.SourceList.Remove(this.CurrentAddItem);
                    }
                }
                finally
                {
                    this.SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, true);
                }
                this._trackingEnumerator = this._sourceCollection.GetEnumerator();
                if (this.CurrentAddItem != null)
                {
                    object obj2 = this.EndAddNew(true);
                    int num2 = -1;
                    if ((this.PageSize > 0) && !this.OnLastLocalPage)
                    {
                        num2 = this.Count - 1;
                    }
                    this.InternalList.Remove(obj2);
                    if (this.IsGrouping)
                    {
                        this._group.RemoveSpecialItem(this._group.Items.Count - 1, obj2, false);
                        if (this.PageSize > 0)
                        {
                            this._temporaryGroup.RemoveSpecialItem(this._temporaryGroup.Items.Count - 1, obj2, false);
                        }
                    }
                    this.OnPropertyChanged("ItemCount");
                    object currentItem = this.CurrentItem;
                    int currentPosition = this.CurrentPosition;
                    bool isCurrentAfterLast = this.IsCurrentAfterLast;
                    bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                    this.AdjustCurrencyForRemove(index);
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, obj2, index));
                    this.RaiseCurrencyChanges(false, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
                    if (num2 > -1)
                    {
                        int num4 = this.ConvertToInternalIndex(num2);
                        object item = null;
                        if (this.IsGrouping)
                        {
                            item = this._temporaryGroup.LeafAt(num4);
                            this._group.AddToSubgroups(item, false);
                        }
                        else
                        {
                            item = this.InternalItemAt(num4);
                        }
                        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, this.IndexOf(item)));
                    }
                }
            }
        }

        private bool CheckFlag(CollectionViewFlags flags)
        {
            return ((this._flags & flags) != 0);
        }

        public void CommitEdit()
        {
            if (this.IsAddingNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringTransaction, new object[] { "CommitEdit", "AddNew" }));
            }
            this.VerifyRefreshNotDeferred();
            if (this.CurrentEditItem != null)
            {
                object currentEditItem = this.CurrentEditItem;
                this.CurrentEditItem = null;
                IEditableObject obj3 = currentEditItem as IEditableObject;
                if (obj3 != null)
                {
                    obj3.EndEdit();
                }
                if (this.UsesLocalArray)
                {
                    int index = this.IndexOf(currentEditItem);
                    int num2 = this.InternalIndexOf(currentEditItem);
                    this._internalList.Remove(currentEditItem);
                    object newCurrentItem = (currentEditItem == this.CurrentItem) ? currentEditItem : null;
                    if ((index >= 0) && this.IsGrouping)
                    {
                        this._group.RemoveItemFromSubgroupsByExhaustiveSearch(currentEditItem);
                        if (this.PageSize > 0)
                        {
                            this._temporaryGroup.RemoveItemFromSubgroupsByExhaustiveSearch(currentEditItem);
                        }
                    }
                    object currentItem = this.CurrentItem;
                    int currentPosition = this.CurrentPosition;
                    bool isCurrentAfterLast = this.IsCurrentAfterLast;
                    bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                    if (index >= 0)
                    {
                        this.AdjustCurrencyForRemove(index);
                        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, currentEditItem, index));
                    }
                    bool flag3 = this.PassesFilter(currentEditItem);
                    if (this.NeedToMoveToPreviousPage && !flag3)
                    {
                        this.MoveToPreviousPage();
                    }
                    else
                    {
                        this.ProcessInsertToCollection(currentEditItem, num2);
                        int num4 = this.PageIndex * this.PageSize;
                        int num5 = num4 + this.PageSize;
                        if (this.IsGrouping)
                        {
                            int num6 = -1;
                            if (flag3 && (this.PageSize > 0))
                            {
                                this._temporaryGroup.AddToSubgroups(currentEditItem, false);
                                num6 = this._temporaryGroup.LeafIndexOf(currentEditItem);
                            }
                            if (flag3 && ((this.PageSize == 0) || ((num4 <= num6) && (num5 > num6))))
                            {
                                this._group.AddToSubgroups(currentEditItem, false);
                                int num7 = this.IndexOf(currentEditItem);
                                this.AdjustCurrencyForEdit(newCurrentItem, num7);
                                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, currentEditItem, num7));
                            }
                            else if (this.PageSize > 0)
                            {
                                int num8 = -1;
                                if (flag3 && (num6 < num4))
                                {
                                    num8 = num4;
                                }
                                else if (!this.OnLastLocalPage && (index >= 0))
                                {
                                    num8 = num5 - 1;
                                }
                                object item = this._temporaryGroup.LeafAt(num8);
                                if (item != null)
                                {
                                    this._group.AddToSubgroups(item, false);
                                    num8 = this.IndexOf(item);
                                    this.AdjustCurrencyForEdit(newCurrentItem, num8);
                                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, num8));
                                }
                            }
                        }
                        else
                        {
                            int num9 = this.IndexOf(currentEditItem);
                            if (num9 >= 0)
                            {
                                this.AdjustCurrencyForEdit(newCurrentItem, num9);
                                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, currentEditItem, num9));
                            }
                            else if (this.PageSize > 0)
                            {
                                bool flag4 = this.PassesFilter(currentEditItem) && (this.InternalIndexOf(currentEditItem) < this.ConvertToInternalIndex(0));
                                num9 = flag4 ? 0 : (this.Count - 1);
                                if (flag4 || (!this.OnLastLocalPage && (index >= 0)))
                                {
                                    this.AdjustCurrencyForEdit(newCurrentItem, num9);
                                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.GetItemAt(num9), num9));
                                }
                            }
                        }
                        this.RaiseCurrencyChanges(true, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
                    }
                }
                else if (!this.Contains(currentEditItem))
                {
                    this.InternalList.Add(currentEditItem);
                }
            }
        }

        public void CommitNew()
        {
            if (this.IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringTransaction, new object[] { "CommitNew", "EditItem" }));
            }
            this.VerifyRefreshNotDeferred();
            if (this.CurrentAddItem != null)
            {
                object obj2 = this.EndAddNew(false);
                object currentItem = this.CurrentItem;
                this._trackingEnumerator = this._sourceCollection.GetEnumerator();
                if (this.UsesLocalArray)
                {
                    int index = this.Count - 1;
                    int num2 = this._internalList.IndexOf(obj2);
                    this._internalList.Remove(obj2);
                    if (this.IsGrouping)
                    {
                        this._group.RemoveSpecialItem(this._group.Items.Count - 1, obj2, false);
                        if (this.PageSize > 0)
                        {
                            this._temporaryGroup.RemoveSpecialItem(this._temporaryGroup.Items.Count - 1, obj2, false);
                        }
                    }
                    object oldCurrentItem = this.CurrentItem;
                    int currentPosition = this.CurrentPosition;
                    bool isCurrentAfterLast = this.IsCurrentAfterLast;
                    bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                    this.AdjustCurrencyForRemove(index);
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, obj2, index));
                    bool flag3 = this.PassesFilter(obj2);
                    this.ProcessInsertToCollection(obj2, num2);
                    int num4 = this.PageIndex * this.PageSize;
                    int num5 = num4 + this.PageSize;
                    if (this.IsGrouping)
                    {
                        int num6 = -1;
                        if (flag3 && (this.PageSize > 0))
                        {
                            this._temporaryGroup.AddToSubgroups(obj2, false);
                            num6 = this._temporaryGroup.LeafIndexOf(obj2);
                        }
                        if (flag3 && ((this.PageSize == 0) || ((num4 <= num6) && (num5 > num6))))
                        {
                            this._group.AddToSubgroups(obj2, false);
                            int num7 = this.IndexOf(obj2);
                            if (currentItem != null)
                            {
                                if (this.Contains(currentItem))
                                {
                                    this.AdjustCurrencyForAdd(currentItem, num7);
                                }
                                else
                                {
                                    this.AdjustCurrencyForAdd(this.GetItemAt(this.Count - 1), num7);
                                }
                            }
                            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, obj2, num7));
                        }
                        else if (!flag3 && ((this.PageSize == 0) || this.OnLastLocalPage))
                        {
                            this.AdjustCurrencyForRemove(index);
                        }
                        else if (this.PageSize > 0)
                        {
                            int num8 = -1;
                            if (flag3 && (num6 < num4))
                            {
                                num8 = num4;
                            }
                            else if (!this.OnLastLocalPage)
                            {
                                num8 = num5 - 1;
                            }
                            object item = this._temporaryGroup.LeafAt(num8);
                            if (item != null)
                            {
                                this._group.AddToSubgroups(item, false);
                                num8 = this.IndexOf(item);
                                if (currentItem != null)
                                {
                                    if (this.Contains(currentItem))
                                    {
                                        this.AdjustCurrencyForAdd(currentItem, num8);
                                    }
                                    else
                                    {
                                        this.AdjustCurrencyForAdd(this.GetItemAt(this.Count - 1), num8);
                                    }
                                }
                                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, num8));
                            }
                        }
                    }
                    else
                    {
                        int num9 = this.IndexOf(obj2);
                        if (num9 >= 0)
                        {
                            this.AdjustCurrencyForAdd(obj2, num9);
                            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, obj2, num9));
                        }
                        else if (!flag3 && ((this.PageSize == 0) || this.OnLastLocalPage))
                        {
                            this.AdjustCurrencyForRemove(index);
                        }
                        else if (this.PageSize > 0)
                        {
                            bool flag4 = this.InternalIndexOf(obj2) < this.ConvertToInternalIndex(0);
                            num9 = flag4 ? 0 : (this.Count - 1);
                            if (flag4 || !this.OnLastLocalPage)
                            {
                                this.AdjustCurrencyForAdd(null, num9);
                                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.GetItemAt(num9), num9));
                            }
                        }
                    }
                    this.RaiseCurrencyChanges(true, oldCurrentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
                }
            }
        }

        private void CompletePageMove(int pageIndex)
        {
            int count = this.Count;
            object currentItem = this.CurrentItem;
            int currentPosition = this.CurrentPosition;
            bool isCurrentAfterLast = this.IsCurrentAfterLast;
            bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
            this._pageIndex = pageIndex;
            if (this.IsGrouping && (this.PageSize > 0))
            {
                this.PrepareGroupsForCurrentPage();
            }
            if (this.Count >= 1)
            {
                this.SetCurrent(this.GetItemAt(0), 0);
            }
            else
            {
                this.SetCurrent(null, -1);
            }
            this.IsPageChanging = false;
            this.OnPropertyChanged("PageIndex");
            this.RaisePageChanged();
            if (this.Count != count)
            {
                this.OnPropertyChanged("Count");
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.RaiseCurrencyChanges(true, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
        }

        public bool Contains(object item)
        {
            this.EnsureCollectionInSync();
            this.VerifyRefreshNotDeferred();
            return (this.IndexOf(item) >= 0);
        }

        private int ConvertToInternalIndex(int index)
        {
            if (this.PageSize > 0)
            {
                return ((this._pageSize * this.PageIndex) + index);
            }
            return index;
        }

        private void CopySourceToInternalList()
        {
            this._internalList = new List<object>();
            IEnumerator enumerator = this.SourceCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this._internalList.Add(enumerator.Current);
            }
        }

        public IDisposable DeferRefresh()
        {
            if (this.IsAddingNew || this.IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringAddOrEdit, new object[] { "DeferRefresh" }));
            }
            this._deferLevel++;
            return new DeferHelper(this);
        }

        public void EditItem(object item)
        {
            this.VerifyRefreshNotDeferred();
            if (this.IsAddingNew)
            {
                if (object.Equals(item, this.CurrentAddItem))
                {
                    return;
                }
                this.CommitNew();
            }
            this.CommitEdit();
            this.CurrentEditItem = item;
            IEditableObject obj2 = item as IEditableObject;
            if (obj2 != null)
            {
                obj2.BeginEdit();
            }
        }

        private object EndAddNew(bool cancel)
        {
            object currentAddItem = this.CurrentAddItem;
            this.CurrentAddItem = null;
            IEditableObject obj3 = currentAddItem as IEditableObject;
            if (obj3 != null)
            {
                if (cancel)
                {
                    obj3.CancelEdit();
                    return currentAddItem;
                }
                obj3.EndEdit();
            }
            return currentAddItem;
        }

        private void EndDefer()
        {
            this._deferLevel--;
            if (this._deferLevel == 0)
            {
                if (this.CheckFlag(CollectionViewFlags.IsUpdatePageSizeDeferred))
                {
                    this.SetFlag(CollectionViewFlags.IsUpdatePageSizeDeferred, false);
                    this.PageSize = this._cachedPageSize;
                }
                if (this.CheckFlag(CollectionViewFlags.IsMoveToPageDeferred))
                {
                    this.SetFlag(CollectionViewFlags.IsMoveToPageDeferred, false);
                    this.MoveToPage(this._cachedPageIndex);
                    this._cachedPageIndex = -1;
                }
                if (this.CheckFlag(CollectionViewFlags.NeedsRefresh))
                {
                    this.Refresh();
                }
            }
        }

        private void EnsureCollectionInSync()
        {
            if (this._pollForChanges)
            {
                try
                {
                    this._trackingEnumerator.MoveNext();
                }
                catch (InvalidOperationException)
                {
                    this._trackingEnumerator = this.SourceCollection.GetEnumerator();
                    this.RefreshOrDefer();
                }
            }
        }

        private void EnsureItemConstructor()
        {
            if (!this._itemConstructorIsValid)
            {
                Type itemType = this.GetItemType(true);
                if (itemType != null)
                {
                    this._itemConstructor = itemType.GetConstructor(Type.EmptyTypes);
                    this._itemConstructorIsValid = true;
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            this.EnsureCollectionInSync();
            this.VerifyRefreshNotDeferred();
            if (this.IsGrouping)
            {
                CollectionViewGroupRoot rootGroup = this.RootGroup;
                if (rootGroup == null)
                {
                    return null;
                }
                return rootGroup.GetLeafEnumerator();
            }
            if (this.PageSize <= 0)
            {
                return new NewItemAwareEnumerator(this, this.InternalList.GetEnumerator(), this.CurrentAddItem);
            }
            List<object> list = new List<object>();
            if (this.PageIndex < 0)
            {
                return list.GetEnumerator();
            }
            for (int i = this._pageSize * this.PageIndex; i < Math.Min(this._pageSize * (this.PageIndex + 1), this.InternalList.Count); i++)
            {
                list.Add(this.InternalList[i]);
            }
            return new NewItemAwareEnumerator(this, list.GetEnumerator(), this.CurrentAddItem);
        }

        public object GetItemAt(int index)
        {
            this.EnsureCollectionInSync();
            this.VerifyRefreshNotDeferred();
            if ((index >= this.Count) || (index < 0))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (this.IsGrouping)
            {
                CollectionViewGroupRoot rootGroup = this.RootGroup;
                if (rootGroup == null)
                {
                    return null;
                }
                return rootGroup.LeafAt(this._isUsingTemporaryGroup ? this.ConvertToInternalIndex(index) : index);
            }
            if ((this.IsAddingNew && this.UsesLocalArray) && (index == (this.Count - 1)))
            {
                return this.CurrentAddItem;
            }
            return this.InternalItemAt(this.ConvertToInternalIndex(index));
        }

        private Type GetItemType(bool useRepresentativeItem)
        {
            foreach (Type type2 in this.SourceCollection.GetType().GetInterfaces())
            {
                if (type2.Name == typeof(IEnumerable<>).Name)
                {
                    Type[] genericArguments = type2.GetGenericArguments();
                    if (genericArguments.Length == 1)
                    {
                        return genericArguments[0];
                    }
                }
            }
            if (useRepresentativeItem)
            {
                object representativeItem = this.GetRepresentativeItem();
                if (representativeItem != null)
                {
                    return representativeItem.GetType();
                }
            }
            return null;
        }

        private object GetRepresentativeItem()
        {
            if (!this.IsEmpty)
            {
                IEnumerator enumerator = this.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current != null)
                    {
                        return current;
                    }
                }
            }
            return null;
        }

        public int IndexOf(object item)
        {
            this.EnsureCollectionInSync();
            this.VerifyRefreshNotDeferred();
            if (this.IsGrouping)
            {
                CollectionViewGroupRoot rootGroup = this.RootGroup;
                if (rootGroup == null)
                {
                    return -1;
                }
                return rootGroup.LeafIndexOf(item);
            }
            if ((this.IsAddingNew && object.Equals(item, this.CurrentAddItem)) && this.UsesLocalArray)
            {
                return (this.Count - 1);
            }
            int num = this.InternalIndexOf(item);
            if ((this.PageSize <= 0) || (num == -1))
            {
                return num;
            }
            if ((num >= (this.PageIndex * this._pageSize)) && (num < ((this.PageIndex + 1) * this._pageSize)))
            {
                return (num - (this.PageIndex * this._pageSize));
            }
            return -1;
        }

        private int InternalIndexOf(object item)
        {
            return this.InternalList.IndexOf(item);
        }

        private object InternalItemAt(int index)
        {
            if ((index >= 0) && (index < this.InternalList.Count))
            {
                return this.InternalList[index];
            }
            return null;
        }

        private static object InvokePath(object item, string propertyPath, Type propertyType)
        {
            Exception exception;
            object obj2 = TypeHelper.GetNestedPropertyValue(item, propertyPath, propertyType, out exception);
            if (exception != null)
            {
                throw exception;
            }
            return obj2;
        }

        public bool MoveCurrentTo(object item)
        {
            this.VerifyRefreshNotDeferred();
            if (!object.Equals(this.CurrentItem, item) || ((item == null) && !this.IsCurrentInView))
            {
                return this.MoveCurrentToPosition(this.IndexOf(item));
            }
            return this.IsCurrentInView;
        }

        public bool MoveCurrentToFirst()
        {
            this.VerifyRefreshNotDeferred();
            return this.MoveCurrentToPosition(0);
        }

        public bool MoveCurrentToLast()
        {
            this.VerifyRefreshNotDeferred();
            int position = this.Count - 1;
            return this.MoveCurrentToPosition(position);
        }

        public bool MoveCurrentToNext()
        {
            this.VerifyRefreshNotDeferred();
            int position = this.CurrentPosition + 1;
            return ((position <= this.Count) && this.MoveCurrentToPosition(position));
        }

        public bool MoveCurrentToPosition(int position)
        {
            this.VerifyRefreshNotDeferred();
            if ((position < -1) || (position > this.Count))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            if (((position != this.CurrentPosition) || !this.IsCurrentInSync) && this.OkToChangeCurrent())
            {
                bool isCurrentAfterLast = this.IsCurrentAfterLast;
                bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                this.SetCurrentToPosition(position);
                this.OnCurrentChanged();
                if (this.IsCurrentAfterLast != isCurrentAfterLast)
                {
                    this.OnPropertyChanged("IsCurrentAfterLast");
                }
                if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
                {
                    this.OnPropertyChanged("IsCurrentBeforeFirst");
                }
                this.OnPropertyChanged("CurrentPosition");
                this.OnPropertyChanged("CurrentItem");
            }
            return this.IsCurrentInView;
        }

        public bool MoveCurrentToPrevious()
        {
            this.VerifyRefreshNotDeferred();
            int position = this.CurrentPosition - 1;
            return ((position >= -1) && this.MoveCurrentToPosition(position));
        }

        public bool MoveToFirstPage()
        {
            return this.MoveToPage(0);
        }

        public bool MoveToLastPage()
        {
            return (((this.TotalItemCount != -1) && (this.PageSize > 0)) && this.MoveToPage(this.PageCount - 1));
        }

        public bool MoveToNextPage()
        {
            return this.MoveToPage(this._pageIndex + 1);
        }

        public bool MoveToPage(int pageIndex)
        {
            if (pageIndex < -1)
            {
                return false;
            }
            if (this.IsRefreshDeferred)
            {
                this._cachedPageIndex = pageIndex;
                this.SetFlag(CollectionViewFlags.IsMoveToPageDeferred, true);
                return false;
            }
            if ((pageIndex == -1) && (this.PageSize > 0))
            {
                return false;
            }
            if ((pageIndex >= this.PageCount) || (this._pageIndex == pageIndex))
            {
                return false;
            }
            if (!this.OkToChangeCurrent())
            {
                return false;
            }
            if (this.RaisePageChanging(pageIndex) && (pageIndex != -1))
            {
                return false;
            }
            if ((this.CurrentAddItem != null) || (this.CurrentEditItem != null))
            {
                object currentItem = this.CurrentItem;
                int currentPosition = this.CurrentPosition;
                bool isCurrentAfterLast = this.IsCurrentAfterLast;
                bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                this.SetCurrentToPosition(-1);
                this.RaiseCurrencyChanges(true, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
                if ((this.CurrentAddItem != null) || (this.CurrentEditItem != null))
                {
                    this.RaisePageChanged();
                    this.SetCurrentToPosition(currentPosition);
                    this.RaiseCurrencyChanges(false, null, -1, true, false);
                    return false;
                }
                this.OnCurrentChanging();
            }
            this.IsPageChanging = true;
            this.CompletePageMove(pageIndex);
            return true;
        }

        public bool MoveToPreviousPage()
        {
            return this.MoveToPage(this._pageIndex - 1);
        }

        private bool OkToChangeCurrent()
        {
            CurrentChangingEventArgs args = new CurrentChangingEventArgs();
            this.OnCurrentChanging(args);
            return !args.Cancel;
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            this._timestamp++;
            if ((this.CollectionChanged != null) && (((args.Action != NotifyCollectionChangedAction.Add) || (this.PageSize == 0)) || (args.NewStartingIndex < this.Count)))
            {
                this.CollectionChanged(this, args);
            }
            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                this.OnPropertyChanged("Count");
            }
            bool isEmpty = this.IsEmpty;
            if (isEmpty != this.CheckFlag(CollectionViewFlags.CachedIsEmpty))
            {
                this.SetFlag(CollectionViewFlags.CachedIsEmpty, isEmpty);
                this.OnPropertyChanged("IsEmpty");
            }
        }

        private void OnCurrentChanged()
        {
            if ((this.CurrentChanged != null) && this._currentChangedMonitor.Enter())
            {
                using (this._currentChangedMonitor)
                {
                    this.CurrentChanged(this, EventArgs.Empty);
                }
            }
        }

        private void OnCurrentChanging()
        {
            this.OnCurrentChanging(uncancelableCurrentChangingEventArgs);
        }

        private void OnCurrentChanging(CurrentChangingEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (this._currentChangedMonitor.Busy)
            {
                if (args.IsCancelable)
                {
                    args.Cancel = true;
                }
            }
            else if (this.CurrentChanging != null)
            {
                this.CurrentChanging(this, args);
            }
        }

        private void OnGroupByChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.IsAddingNew || this.IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringAddOrEdit, new object[] { "Grouping" }));
            }
            this.RefreshOrDefer();
        }

        private void OnGroupDescriptionChanged(object sender, EventArgs e)
        {
            if (this.IsAddingNew || this.IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringAddOrEdit, new object[] { "Grouping" }));
            }
            this.RefreshOrDefer();
            if (this.PageSize > 0)
            {
                if (this.IsRefreshDeferred)
                {
                    this._cachedPageIndex = 0;
                    this.SetFlag(CollectionViewFlags.IsMoveToPageDeferred, true);
                }
                else
                {
                    this.MoveToFirstPage();
                }
            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public bool PassesFilter(object item)
        {
            if (this.Filter != null)
            {
                return this.Filter(item);
            }
            return true;
        }

        private void PrepareGroupingComparer(CollectionViewGroupRoot groupRoot)
        {
            if ((groupRoot == this._temporaryGroup) || (this.PageSize == 0))
            {
                CollectionViewGroupInternal.ListComparer activeComparer = groupRoot.ActiveComparer as CollectionViewGroupInternal.ListComparer;
                if (activeComparer != null)
                {
                    activeComparer.ResetList(this.InternalList);
                }
                else
                {
                    groupRoot.ActiveComparer = new CollectionViewGroupInternal.ListComparer(this.InternalList);
                }
            }
            else if (groupRoot == this._group)
            {
                groupRoot.ActiveComparer = new CollectionViewGroupInternal.CollectionViewGroupComparer(this._temporaryGroup);
            }
        }

        private void PrepareGroups()
        {
            this._group.Clear();
            this._group.Initialize();
            this._group.IsDataInGroupOrder = this.CheckFlag(CollectionViewFlags.IsDataInGroupOrder);
            this._isGrouping = false;
            if (this._group.GroupDescriptions.Count > 0)
            {
                int num = 0;
                int count = this._internalList.Count;
                while (num < count)
                {
                    object objB = this._internalList[num];
                    if ((objB != null) && (!this.IsAddingNew || !object.Equals(this.CurrentAddItem, objB)))
                    {
                        this._group.AddToSubgroups(objB, true);
                    }
                    num++;
                }
                if (this.IsAddingNew)
                {
                    this._group.InsertSpecialItem(this._group.Items.Count, this.CurrentAddItem, true);
                }
            }
            this._isGrouping = this._group.GroupBy != null;
            this._group.IsDataInGroupOrder = false;
            this.PrepareGroupingComparer(this._group);
        }

        private void PrepareGroupsForCurrentPage()
        {
            this._group.Clear();
            this._group.Initialize();
            this._isUsingTemporaryGroup = true;
            this._group.IsDataInGroupOrder = true;
            this._group.ActiveComparer = null;
            if (this.GroupDescriptions.Count > 0)
            {
                int index = 0;
                int count = this.Count;
                while (index < count)
                {
                    object itemAt = this.GetItemAt(index);
                    if ((itemAt != null) && (!this.IsAddingNew || !object.Equals(this.CurrentAddItem, itemAt)))
                    {
                        this._group.AddToSubgroups(itemAt, true);
                    }
                    index++;
                }
                if (this.IsAddingNew)
                {
                    this._group.InsertSpecialItem(this._group.Items.Count, this.CurrentAddItem, true);
                }
            }
            this._isUsingTemporaryGroup = false;
            this._group.IsDataInGroupOrder = false;
            this.PrepareGroupingComparer(this._group);
            this._isGrouping = this._group.GroupBy != null;
        }

        private IList PrepareLocalArray(IEnumerable enumerable)
        {
            List<object> list = new List<object>();
            foreach (object obj2 in enumerable)
            {
                if ((this.Filter == null) || this.PassesFilter(obj2))
                {
                    list.Add(obj2);
                }
            }
            if (!this.CheckFlag(CollectionViewFlags.IsDataSorted) && (this.SortDescriptions.Count > 0))
            {
                list = this.SortList(list);
            }
            return list;
        }

        private void PrepareTemporaryGroups()
        {
            this._temporaryGroup = new CollectionViewGroupRoot(this, this.CheckFlag(CollectionViewFlags.IsDataInGroupOrder));
            foreach (GroupDescription description in this._group.GroupDescriptions)
            {
                this._temporaryGroup.GroupDescriptions.Add(description);
            }
            this._temporaryGroup.Initialize();
            this._isGrouping = false;
            if (this._temporaryGroup.GroupDescriptions.Count > 0)
            {
                int num = 0;
                int count = this._internalList.Count;
                while (num < count)
                {
                    object objB = this._internalList[num];
                    if ((objB != null) && (!this.IsAddingNew || !object.Equals(this.CurrentAddItem, objB)))
                    {
                        this._temporaryGroup.AddToSubgroups(objB, true);
                    }
                    num++;
                }
                if (this.IsAddingNew)
                {
                    this._temporaryGroup.InsertSpecialItem(this._temporaryGroup.Items.Count, this.CurrentAddItem, true);
                }
            }
            this._isGrouping = this._temporaryGroup.GroupBy != null;
            this.PrepareGroupingComparer(this._temporaryGroup);
        }

        private void ProcessAddEvent(object addedItem, int addIndex)
        {
            object item = null;
            if ((this.PageSize > 0) && !this.IsGrouping)
            {
                item = (this.Count == this.PageSize) ? this.GetItemAt(this.PageSize - 1) : null;
            }
            this.ProcessInsertToCollection(addedItem, addIndex);
            bool flag = false;
            if ((this.Count == 1) && (this.GroupDescriptions.Count > 0))
            {
                if (this.PageSize > 0)
                {
                    this.PrepareGroupingComparer(this._temporaryGroup);
                }
                this.PrepareGroupingComparer(this._group);
            }
            if (this.IsGrouping)
            {
                int num = -1;
                if (this.PageSize > 0)
                {
                    this._temporaryGroup.AddToSubgroups(addedItem, false);
                    num = this._temporaryGroup.LeafIndexOf(addedItem);
                }
                if ((this.PageSize == 0) || (((this.PageIndex + 1) * this.PageSize) > num))
                {
                    flag = true;
                    int num2 = this.PageIndex * this.PageSize;
                    if ((num2 > num) && (this.PageSize > 0))
                    {
                        addedItem = this._temporaryGroup.LeafAt(num2);
                    }
                    if ((this.PageSize > 0) && (this._group.ItemCount == this.PageSize))
                    {
                        item = this._group.LeafAt(this.PageSize - 1);
                        this._group.RemoveFromSubgroups(item);
                    }
                }
            }
            if ((((this.PageSize > 0) && !this.OnLastLocalPage) && ((this.IsGrouping && (item != null)) || (!this.IsGrouping && (((this.PageIndex + 1) * this.PageSize) > this.InternalIndexOf(addedItem))))) && ((item != null) && (item != addedItem)))
            {
                this.AdjustCurrencyForRemove(this.PageSize - 1);
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, this.PageSize - 1));
            }
            if (flag)
            {
                this._group.AddToSubgroups(addedItem, false);
            }
            int index = this.IndexOf(addedItem);
            if (index >= 0)
            {
                object currentItem = this.CurrentItem;
                int currentPosition = this.CurrentPosition;
                bool isCurrentAfterLast = this.IsCurrentAfterLast;
                bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                this.AdjustCurrencyForAdd(null, index);
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItem, index));
                this.RaiseCurrencyChanges(false, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
            }
            else if (this.PageSize > 0)
            {
                int num5 = this.IsGrouping ? this._group.LeafIndexOf(addedItem) : this.InternalIndexOf(addedItem);
                if (num5 < this.ConvertToInternalIndex(0))
                {
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.GetItemAt(0), 0));
                }
            }
        }

        private void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (this.CheckFlag(CollectionViewFlags.ShouldProcessCollectionChanged))
            {
                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    if (!this.SourceCollection.GetEnumerator().MoveNext())
                    {
                        this._internalList.Clear();
                    }
                    this.RefreshOrDefer();
                }
                else
                {
                    object item = (args.NewItems != null) ? args.NewItems[0] : null;
                    object removedItem = (args.OldItems != null) ? args.OldItems[0] : null;
                    if ((args.Action == NotifyCollectionChangedAction.Remove) || (args.Action == NotifyCollectionChangedAction.Replace))
                    {
                        this.ProcessRemoveEvent(removedItem, args.Action == NotifyCollectionChangedAction.Replace);
                    }
                    if (((args.Action == NotifyCollectionChangedAction.Add) || (args.Action == NotifyCollectionChangedAction.Replace)) && ((this.Filter == null) || this.PassesFilter(item)))
                    {
                        this.ProcessAddEvent(item, args.NewStartingIndex);
                    }
                    if (args.Action != NotifyCollectionChangedAction.Replace)
                    {
                        this.OnPropertyChanged("ItemCount");
                    }
                }
            }
        }

        private void ProcessInsertToCollection(object item, int index)
        {
            if ((this.Filter == null) || this.PassesFilter(item))
            {
                if (this.SortDescriptions.Count > 0)
                {
                    SortFieldComparer comparer = new SortFieldComparer(this.SortDescriptions);
                    if (((index < 0) || ((index > 0) && (comparer.Compare(item, this.InternalItemAt(index - 1)) < 0))) || ((index < (this.InternalList.Count - 1)) && (comparer.Compare(item, this.InternalItemAt(index)) > 0)))
                    {
                        index = comparer.FindInsertIndex(item, this._internalList);
                    }
                }
                if ((index < 0) || (index > this._internalList.Count))
                {
                    index = this._internalList.Count;
                }
                this._internalList.Insert(index, item);
            }
        }

        private void ProcessRemoveEvent(object removedItem, bool isReplace)
        {
            int num = -1;
            if (this.IsGrouping)
            {
                num = (this.PageSize > 0) ? this._temporaryGroup.LeafIndexOf(removedItem) : this._group.LeafIndexOf(removedItem);
            }
            else
            {
                num = this.InternalIndexOf(removedItem);
            }
            int index = this.IndexOf(removedItem);
            this._internalList.Remove(removedItem);
            bool flag = ((this.PageSize == 0) && (index >= 0)) || (num < ((this.PageIndex + 1) * this.PageSize));
            if (this.IsGrouping)
            {
                if (this.PageSize > 0)
                {
                    this._temporaryGroup.RemoveFromSubgroups(removedItem);
                }
                if (flag)
                {
                    this._group.RemoveFromSubgroups((index >= 0) ? removedItem : this._group.LeafAt(0));
                }
            }
            if (flag)
            {
                object currentItem = this.CurrentItem;
                int currentPosition = this.CurrentPosition;
                bool isCurrentAfterLast = this.IsCurrentAfterLast;
                bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                this.AdjustCurrencyForRemove(index);
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, Math.Max(0, index)));
                this.RaiseCurrencyChanges(false, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
                if (this.NeedToMoveToPreviousPage && !isReplace)
                {
                    this.MoveToPreviousPage();
                }
                else if ((this.PageSize > 0) && (this.Count == this.PageSize))
                {
                    if (this.IsGrouping)
                    {
                        object item = this._temporaryGroup.LeafAt((this.PageSize * (this.PageIndex + 1)) - 1);
                        if (item != null)
                        {
                            this._group.AddToSubgroups(item, false);
                        }
                    }
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.GetItemAt(this.PageSize - 1), this.PageSize - 1));
                }
            }
        }

        private void RaiseCurrencyChanges(bool fireChangedEvent, object oldCurrentItem, int oldCurrentPosition, bool oldIsCurrentBeforeFirst, bool oldIsCurrentAfterLast)
        {
            if ((fireChangedEvent || (this.CurrentItem != oldCurrentItem)) || (this.CurrentPosition != oldCurrentPosition))
            {
                this.OnCurrentChanged();
            }
            if (this.CurrentItem != oldCurrentItem)
            {
                this.OnPropertyChanged("CurrentItem");
            }
            if (this.CurrentPosition != oldCurrentPosition)
            {
                this.OnPropertyChanged("CurrentPosition");
            }
            if (this.IsCurrentAfterLast != oldIsCurrentAfterLast)
            {
                this.OnPropertyChanged("IsCurrentAfterLast");
            }
            if (this.IsCurrentBeforeFirst != oldIsCurrentBeforeFirst)
            {
                this.OnPropertyChanged("IsCurrentBeforeFirst");
            }
        }

        private void RaisePageChanged()
        {
            EventHandler<EventArgs> pageChanged = this.PageChanged;
            if (pageChanged != null)
            {
                pageChanged(this, EventArgs.Empty);
            }
        }

        private bool RaisePageChanging(int newPageIndex)
        {
            EventHandler<PageChangingEventArgs> pageChanging = this.PageChanging;
            if (pageChanging != null)
            {
                PageChangingEventArgs e = new PageChangingEventArgs(newPageIndex);
                pageChanging(this, e);
                return e.Cancel;
            }
            return false;
        }

        public void Refresh()
        {
            IEditableCollectionView view = this;
            if ((view != null) && (view.IsAddingNew || view.IsEditingItem))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringAddOrEdit, new object[] { "Refresh" }));
            }
            this.RefreshInternal();
        }

        private void RefreshInternal()
        {
            this.RefreshOverride();
            this.SetFlag(CollectionViewFlags.NeedsRefresh, false);
        }

        private void RefreshOrDefer()
        {
            if (this.IsRefreshDeferred)
            {
                this.SetFlag(CollectionViewFlags.NeedsRefresh, true);
            }
            else
            {
                this.RefreshInternal();
            }
        }

        private void RefreshOverride()
        {
            object currentItem = this.CurrentItem;
            int currentPosition = this.CurrentPosition;
            bool isCurrentAfterLast = this.IsCurrentAfterLast;
            bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
            this._isGrouping = false;
            this.OnCurrentChanging();
            if (this.UsesLocalArray)
            {
                try
                {
                    this._internalList = this.PrepareLocalArray(this._sourceCollection);
                    if (this.PageSize == 0)
                    {
                        this.PrepareGroups();
                    }
                    else
                    {
                        this.PrepareTemporaryGroups();
                        this.PrepareGroupsForCurrentPage();
                    }
                    goto Label_007C;
                }
                catch (TargetInvocationException exception)
                {
                    if (exception.InnerException != null)
                    {
                        throw exception.InnerException;
                    }
                    throw;
                }
            }
            this.CopySourceToInternalList();
        Label_007C:
            if (((this.PageSize > 0) && (this.PageIndex > 0)) && (this.PageIndex >= this.PageCount))
            {
                this.MoveToPage(this.PageCount - 1);
            }
            this.ResetCurrencyValues(currentItem, isCurrentBeforeFirst, isCurrentAfterLast);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.RaiseCurrencyChanges(false, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
        }

        public void Remove(object item)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException("index", PagedCollectionViewResources.IndexOutOfRange);
            }
            if (this.IsEditingItem || this.IsAddingNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringAddOrEdit, new object[] { "RemoveAt" }));
            }
            if (!this.CanRemove)
            {
                throw new InvalidOperationException(PagedCollectionViewResources.RemoveNotSupported);
            }
            this.VerifyRefreshNotDeferred();
            object itemAt = this.GetItemAt(index);
            bool flag = (this.PageSize > 0) && !this.OnLastLocalPage;
            try
            {
                this.SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, false);
                if (this.SourceList != null)
                {
                    this.SourceList.Remove(itemAt);
                }
            }
            finally
            {
                this.SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, true);
            }
            this._trackingEnumerator = this._sourceCollection.GetEnumerator();
            this._internalList.Remove(itemAt);
            if (this.IsGrouping)
            {
                if (this.PageSize > 0)
                {
                    this._temporaryGroup.RemoveFromSubgroups(itemAt);
                }
                this._group.RemoveFromSubgroups(itemAt);
            }
            object currentItem = this.CurrentItem;
            int currentPosition = this.CurrentPosition;
            bool isCurrentAfterLast = this.IsCurrentAfterLast;
            bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
            this.AdjustCurrencyForRemove(index);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, itemAt, index));
            this.RaiseCurrencyChanges(false, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
            if (this.NeedToMoveToPreviousPage)
            {
                this.MoveToPreviousPage();
            }
            else if (flag)
            {
                if (this.IsGrouping)
                {
                    object item = this._temporaryGroup.LeafAt((this.PageSize * (this.PageIndex + 1)) - 1);
                    if (item != null)
                    {
                        this._group.AddToSubgroups(item, false);
                    }
                }
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.GetItemAt(this.PageSize - 1), this.PageSize - 1));
            }
        }

        private void ResetCurrencyValues(object oldCurrentItem, bool oldIsCurrentBeforeFirst, bool oldIsCurrentAfterLast)
        {
            if (oldIsCurrentBeforeFirst || this.IsEmpty)
            {
                this.SetCurrent(null, -1);
            }
            else if (oldIsCurrentAfterLast)
            {
                this.SetCurrent(null, this.Count);
            }
            else
            {
                int index = this.IndexOf(oldCurrentItem);
                if (index < 0)
                {
                    index = 0;
                    if (index < this.Count)
                    {
                        this.SetCurrent(this.GetItemAt(index), index);
                    }
                    else if (!this.IsEmpty)
                    {
                        this.SetCurrent(this.GetItemAt(0), 0);
                    }
                    else
                    {
                        this.SetCurrent(null, -1);
                    }
                }
                else
                {
                    this.SetCurrent(oldCurrentItem, index);
                }
            }
        }

        private void SetCurrent(object newItem, int newPosition)
        {
            int count = (newItem != null) ? 0 : (this.IsEmpty ? 0 : this.Count);
            this.SetCurrent(newItem, newPosition, count);
        }

        private void SetCurrent(object newItem, int newPosition, int count)
        {
            if (newItem != null)
            {
                this.SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, false);
                this.SetFlag(CollectionViewFlags.IsCurrentAfterLast, false);
            }
            else if (count == 0)
            {
                this.SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, true);
                this.SetFlag(CollectionViewFlags.IsCurrentAfterLast, true);
                newPosition = -1;
            }
            else
            {
                this.SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, newPosition < 0);
                this.SetFlag(CollectionViewFlags.IsCurrentAfterLast, newPosition >= count);
            }
            this._currentItem = newItem;
            this._currentPosition = newPosition;
        }

        private void SetCurrentToPosition(int position)
        {
            if (position < 0)
            {
                this.SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, true);
                this.SetCurrent(null, -1);
            }
            else if (position >= this.Count)
            {
                this.SetFlag(CollectionViewFlags.IsCurrentAfterLast, true);
                this.SetCurrent(null, this.Count);
            }
            else
            {
                this.SetFlag(CollectionViewFlags.IsCurrentAfterLast | CollectionViewFlags.IsCurrentBeforeFirst, false);
                this.SetCurrent(this.GetItemAt(position), position);
            }
        }

        private void SetFlag(CollectionViewFlags flags, bool value)
        {
            if (value)
            {
                this._flags |= flags;
            }
            else
            {
                this._flags &= ~flags;
            }
        }

        private void SetSortDescriptions(SortDescriptionCollection descriptions)
        {
            if (this._sortDescriptions != null)
            {
                this._sortDescriptions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
            }
            this._sortDescriptions = descriptions;
            if (this._sortDescriptions != null)
            {
                this._sortDescriptions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
            }
        }

        private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.IsAddingNew || this.IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringAddOrEdit, new object[] { "Sorting" }));
            }
            this.RefreshOrDefer();
            if (this.PageSize > 0)
            {
                if (this.IsRefreshDeferred)
                {
                    this._cachedPageIndex = 0;
                    this.SetFlag(CollectionViewFlags.IsMoveToPageDeferred, true);
                }
                else
                {
                    this.MoveToFirstPage();
                }
            }
            this.OnPropertyChanged("SortDescriptions");
        }

        private List<object> SortList(List<object> list)
        {
            IEnumerable<object> source = list;
            foreach (SortDescription description in this.SortDescriptions)
            {
                Func<object, object> keySelector = null;
                Func<object, object> func2 = null;
                Func<object, object> func3 = null;
                Func<object, object> func4 = null;
                string propertyPath = description.PropertyName;
                Type propertyType = null;
                foreach (object obj2 in list)
                {
                    if (obj2 != null)
                    {
                        propertyType = obj2.GetType().GetNestedPropertyType(propertyPath);
                        break;
                    }
                }
                IOrderedEnumerable<object> enumerable2 = source as IOrderedEnumerable<object>;
                switch (description.Direction)
                {
                    case ListSortDirection.Ascending:
                    {
                        if (enumerable2 == null)
                        {
                            break;
                        }
                        if (keySelector == null)
                        {
                            keySelector = delegate (object item) {
                                return InvokePath(item, propertyPath, propertyType);
                            };
                        }
                        source = enumerable2.ThenBy<object, object>(keySelector);
                        continue;
                    }
                    case ListSortDirection.Descending:
                    {
                        if (enumerable2 == null)
                        {
                            goto Label_010F;
                        }
                        if (func3 == null)
                        {
                            func3 = delegate (object item) {
                                return InvokePath(item, propertyPath, propertyType);
                            };
                        }
                        source = enumerable2.ThenByDescending<object, object>(func3);
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
                if (func2 == null)
                {
                    func2 = delegate (object item) {
                        return InvokePath(item, propertyPath, propertyType);
                    };
                }
                source = source.OrderBy<object, object>(func2);
                continue;
            Label_010F:
                if (func4 == null)
                {
                    func4 = delegate (object item) {
                        return InvokePath(item, propertyPath, propertyType);
                    };
                }
                source = source.OrderByDescending<object, object>(func4);
            }
            return source.ToList<object>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void VerifyRefreshNotDeferred()
        {
            if (this.IsRefreshDeferred)
            {
                throw new InvalidOperationException(PagedCollectionViewResources.NoCheckOrChangeWhenDeferred);
            }
        }

        // Properties
        public bool CanAddNew
        {
            get
            {
                if (this.IsEditingItem)
                {
                    return false;
                }
                return (((this.SourceList != null) && !this.SourceList.IsFixedSize) && this.CanConstructItem);
            }
        }

        public bool CanCancelEdit
        {
            get
            {
                return (this._editItem is IEditableObject);
            }
        }

        public bool CanChangePage
        {
            get
            {
                return true;
            }
        }

        private bool CanConstructItem
        {
            get
            {
                if (!this._itemConstructorIsValid)
                {
                    this.EnsureItemConstructor();
                }
                return (this._itemConstructor != null);
            }
        }

        public bool CanFilter
        {
            get
            {
                return true;
            }
        }

        public bool CanGroup
        {
            get
            {
                return true;
            }
        }

        public bool CanRemove
        {
            get
            {
                if (this.IsEditingItem || this.IsAddingNew)
                {
                    return false;
                }
                return ((this.SourceList != null) && !this.SourceList.IsFixedSize);
            }
        }

        public bool CanSort
        {
            get
            {
                return true;
            }
        }

        public int Count
        {
            get
            {
                this.EnsureCollectionInSync();
                this.VerifyRefreshNotDeferred();
                if ((this.PageSize > 0) && (this.PageIndex > -1))
                {
                    if (this.IsGrouping && !this._isUsingTemporaryGroup)
                    {
                        return this._group.ItemCount;
                    }
                    return Math.Max(0, Math.Min(this.PageSize, this.InternalCount - (this._pageSize * this.PageIndex)));
                }
                if (!this.IsGrouping)
                {
                    return this.InternalCount;
                }
                if (this._isUsingTemporaryGroup)
                {
                    return this._temporaryGroup.ItemCount;
                }
                return this._group.ItemCount;
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return this._culture;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this._culture != value)
                {
                    this._culture = value;
                    this.OnPropertyChanged("Culture");
                }
            }
        }

        public object CurrentAddItem
        {
            get
            {
                return this._newItem;
            }
            private set
            {
                if (this._newItem != value)
                {
                    this._newItem = value;
                    this.OnPropertyChanged("IsAddingNew");
                    this.OnPropertyChanged("CurrentAddItem");
                }
            }
        }

        public object CurrentEditItem
        {
            get
            {
                return this._editItem;
            }
            private set
            {
                if (this._editItem != value)
                {
                    bool canCancelEdit = this.CanCancelEdit;
                    this._editItem = value;
                    this.OnPropertyChanged("IsEditingItem");
                    this.OnPropertyChanged("CurrentEditItem");
                    if (canCancelEdit != this.CanCancelEdit)
                    {
                        this.OnPropertyChanged("CanCancelEdit");
                    }
                }
            }
        }

        public object CurrentItem
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this._currentItem;
            }
        }

        public int CurrentPosition
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this._currentPosition;
            }
        }

        public Predicate<object> Filter
        {
            get
            {
                return this._filter;
            }
            set
            {
                if (this.IsAddingNew || this.IsEditingItem)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.OperationNotAllowedDuringAddOrEdit, new object[] { "Filter" }));
                }
                if (!this.CanFilter)
                {
                    throw new NotSupportedException(PagedCollectionViewResources.CannotFilter);
                }
                if (this._filter != value)
                {
                    this._filter = value;
                    this.RefreshOrDefer();
                    this.OnPropertyChanged("Filter");
                }
            }
        }

        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get
            {
                if (this._group == null)
                {
                    return null;
                }
                return this._group.GroupDescriptions;
            }
        }

        public ReadOnlyObservableCollection<object> Groups
        {
            get
            {
                if (!this.IsGrouping)
                {
                    return null;
                }
                CollectionViewGroupRoot rootGroup = this.RootGroup;
                if (rootGroup == null)
                {
                    return null;
                }
                return rootGroup.Items;
            }
        }

        private int InternalCount
        {
            get
            {
                return this.InternalList.Count;
            }
        }

        private IList InternalList
        {
            get
            {
                return this._internalList;
            }
        }

        public bool IsAddingNew
        {
            get
            {
                return (this._newItem != null);
            }
        }

        public bool IsCurrentAfterLast
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this.CheckFlag(CollectionViewFlags.IsCurrentAfterLast);
            }
        }

        public bool IsCurrentBeforeFirst
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this.CheckFlag(CollectionViewFlags.IsCurrentBeforeFirst);
            }
        }

        private bool IsCurrentInSync
        {
            get
            {
                if (this.IsCurrentInView)
                {
                    return this.GetItemAt(this.CurrentPosition).Equals(this.CurrentItem);
                }
                return (this.CurrentItem == null);
            }
        }

        private bool IsCurrentInView
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return (this.IndexOf(this.CurrentItem) >= 0);
            }
        }

        public bool IsEditingItem
        {
            get
            {
                return (this._editItem != null);
            }
        }

        public bool IsEmpty
        {
            get
            {
                this.EnsureCollectionInSync();
                return (this.InternalCount == 0);
            }
        }

        private bool IsGrouping
        {
            get
            {
                return this._isGrouping;
            }
        }

        public bool IsPageChanging
        {
            get
            {
                return this.CheckFlag(CollectionViewFlags.IsPageChanging);
            }
            private set
            {
                if (this.CheckFlag(CollectionViewFlags.IsPageChanging) != value)
                {
                    this.SetFlag(CollectionViewFlags.IsPageChanging, value);
                    this.OnPropertyChanged("IsPageChanging");
                }
            }
        }

        private bool IsRefreshDeferred
        {
            get
            {
                return (this._deferLevel > 0);
            }
        }

        public object this[int index]
        {
            get
            {
                return this.GetItemAt(index);
            }
        }

        public int ItemCount
        {
            get
            {
                return this.InternalList.Count;
            }
        }

        public bool NeedsRefresh
        {
            get
            {
                return this.CheckFlag(CollectionViewFlags.NeedsRefresh);
            }
        }

        private bool NeedToMoveToPreviousPage
        {
            get
            {
                return ((((this.PageSize > 0) && (this.Count == 0)) && (this.PageIndex != 0)) && (this.PageCount == this.PageIndex));
            }
        }

        public NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get
            {
                return NewItemPlaceholderPosition.None;
            }
            set
            {
                if (value != NewItemPlaceholderPosition.None)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, PagedCollectionViewResources.InvalidEnumArgument, new object[] { "value", value.ToString(), typeof(NewItemPlaceholderPosition).Name }));
                }
            }
        }

        private bool OnLastLocalPage
        {
            get
            {
                if (this.PageSize == 0)
                {
                    return false;
                }
                return ((this.PageCount == 1) || (this.PageIndex == (this.PageCount - 1)));
            }
        }

        private int PageCount
        {
            get
            {
                if (this._pageSize <= 0)
                {
                    return 0;
                }
                return Math.Max(1, (int) Math.Ceiling((double) (((double) this.ItemCount) / ((double) this._pageSize))));
            }
        }

        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }
        }

        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(PagedCollectionViewResources.InvalidPageSize);
                }
                if (this.IsRefreshDeferred)
                {
                    this._cachedPageSize = value;
                    this.SetFlag(CollectionViewFlags.IsUpdatePageSizeDeferred, true);
                }
                else
                {
                    int count = this.Count;
                    if (this._pageSize != value)
                    {
                        object currentItem = this.CurrentItem;
                        int currentPosition = this.CurrentPosition;
                        bool isCurrentAfterLast = this.IsCurrentAfterLast;
                        bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                        if ((this.CurrentAddItem != null) || (this.CurrentEditItem != null))
                        {
                            if (!this.OkToChangeCurrent())
                            {
                                throw new InvalidOperationException(PagedCollectionViewResources.ChangingPageSizeNotAllowedDuringAddOrEdit);
                            }
                            this.SetCurrentToPosition(-1);
                            this.RaiseCurrencyChanges(true, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
                            if ((this.CurrentAddItem != null) || (this.CurrentEditItem != null))
                            {
                                throw new InvalidOperationException(PagedCollectionViewResources.ChangingPageSizeNotAllowedDuringAddOrEdit);
                            }
                        }
                        this._pageSize = value;
                        this.OnPropertyChanged("PageSize");
                        if (this._pageSize == 0)
                        {
                            this.PrepareGroups();
                            this.MoveToPage(-1);
                        }
                        else if (this._pageIndex != 0)
                        {
                            if (!this.CheckFlag(CollectionViewFlags.IsMoveToPageDeferred))
                            {
                                if (this.IsGrouping && (this._temporaryGroup.ItemCount != this.InternalList.Count))
                                {
                                    this.PrepareTemporaryGroups();
                                }
                                this.MoveToFirstPage();
                            }
                        }
                        else if (this.IsGrouping)
                        {
                            if (this._temporaryGroup.ItemCount != this.InternalList.Count)
                            {
                                this.PrepareTemporaryGroups();
                            }
                            this.PrepareGroupsForCurrentPage();
                        }
                        if (this.Count != count)
                        {
                            this.OnPropertyChanged("Count");
                        }
                        this.ResetCurrencyValues(currentItem, isCurrentBeforeFirst, isCurrentAfterLast);
                        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        this.RaiseCurrencyChanges(false, currentItem, currentPosition, isCurrentBeforeFirst, isCurrentAfterLast);
                    }
                }
            }
        }

        private CollectionViewGroupRoot RootGroup
        {
            get
            {
                if (!this._isUsingTemporaryGroup)
                {
                    return this._group;
                }
                return this._temporaryGroup;
            }
        }

        public SortDescriptionCollection SortDescriptions
        {
            get
            {
                if (this._sortDescriptions == null)
                {
                    this.SetSortDescriptions(new SortDescriptionCollection());
                }
                return this._sortDescriptions;
            }
        }

        public IEnumerable SourceCollection
        {
            get
            {
                return this._sourceCollection;
            }
        }

        private IList SourceList
        {
            get
            {
                return (this.SourceCollection as IList);
            }
        }

        private int Timestamp
        {
            get
            {
                return this._timestamp;
            }
        }

        public int TotalItemCount
        {
            get
            {
                return this.InternalList.Count;
            }
        }

        private bool UsesLocalArray
        {
            get
            {
                if (((this.SortDescriptions.Count <= 0) && (this.Filter == null)) && (this._pageSize <= 0))
                {
                    return (this.GroupDescriptions.Count > 0);
                }
                return true;
            }
        }

        // Nested Types
        [Flags]
        private enum CollectionViewFlags
        {
            CachedIsEmpty = 0x40,
            IsCurrentAfterLast = 0x10,
            IsCurrentBeforeFirst = 8,
            IsDataInGroupOrder = 1,
            IsDataSorted = 2,
            IsMoveToPageDeferred = 0x100,
            IsPageChanging = 0x80,
            IsUpdatePageSizeDeferred = 0x200,
            NeedsRefresh = 0x20,
            ShouldProcessCollectionChanged = 4
        }

        private class DeferHelper : IDisposable
        {
            // Fields
            private PagedCollectionView collectionView;

            // Methods
            public DeferHelper(PagedCollectionView collectionView)
            {
                this.collectionView = collectionView;
            }

            public void Dispose()
            {
                if (this.collectionView != null)
                {
                    this.collectionView.EndDefer();
                    this.collectionView = null;
                }
                GC.SuppressFinalize(this);
            }
        }

        private class NewItemAwareEnumerator : IEnumerator
        {
            // Fields
            private IEnumerator _baseEnumerator;
            private PagedCollectionView _collectionView;
            private object _newItem;
            private Position _position;
            private int _timestamp;

            // Methods
            public NewItemAwareEnumerator(PagedCollectionView collectionView, IEnumerator baseEnumerator, object newItem)
            {
                this._collectionView = collectionView;
                this._timestamp = collectionView.Timestamp;
                this._baseEnumerator = baseEnumerator;
                this._newItem = newItem;
            }

            public bool MoveNext()
            {
                if (this._timestamp != this._collectionView.Timestamp)
                {
                    throw new InvalidOperationException(PagedCollectionViewResources.EnumeratorVersionChanged);
                }
                if (this._position == Position.BeforeNewItem)
                {
                    if (!this._baseEnumerator.MoveNext() || (((this._newItem != null) && (this._baseEnumerator.Current == this._newItem)) && !this._baseEnumerator.MoveNext()))
                    {
                        if (this._newItem == null)
                        {
                            return false;
                        }
                        this._position = Position.OnNewItem;
                    }
                    return true;
                }
                this._position = Position.AfterNewItem;
                if (!this._baseEnumerator.MoveNext())
                {
                    return false;
                }
                if ((this._newItem != null) && (this._baseEnumerator.Current == this._newItem))
                {
                    return this._baseEnumerator.MoveNext();
                }
                return true;
            }

            public void Reset()
            {
                this._position = Position.BeforeNewItem;
                this._baseEnumerator.Reset();
            }

            // Properties
            public object Current
            {
                get
                {
                    if (this._position != Position.OnNewItem)
                    {
                        return this._baseEnumerator.Current;
                    }
                    return this._newItem;
                }
            }

            // Nested Types
            private enum Position
            {
                BeforeNewItem,
                OnNewItem,
                AfterNewItem
            }
        }

        private delegate void RequestPageMoveDelegate(int pageIndex);

        private class SimpleMonitor : IDisposable
        {
            // Fields
            private bool entered;

            // Methods
            public void Dispose()
            {
                this.entered = false;
                GC.SuppressFinalize(this);
            }

            public bool Enter()
            {
                if (this.entered)
                {
                    return false;
                }
                this.entered = true;
                return true;
            }

            // Properties
            public bool Busy
            {
                get
                {
                    return this.entered;
                }
            }
        }

        internal class SortFieldComparer : IComparer
        {
            // Fields
            private SortPropertyInfo[] _fields;
            private SortDescriptionCollection _sortFields;

            // Methods
            internal SortFieldComparer()
            {
            }

            public SortFieldComparer(SortDescriptionCollection sortFields)
            {
                this._sortFields = sortFields;
                this._fields = CreatePropertyInfo(this._sortFields);
            }

            public int Compare(object x, object y)
            {
                int num = 0;
                for (int i = 0; i < this._fields.Length; i++)
                {
                    Type propertyType = this._fields[i].PropertyType;
                    if (propertyType == null)
                    {
                        if (x != null)
                        {
                            this._fields[i].PropertyType = x.GetType().GetNestedPropertyType(this._fields[i].PropertyPath);
                            propertyType = this._fields[i].PropertyType;
                        }
                        if ((this._fields[i].PropertyType == null) && (y != null))
                        {
                            this._fields[i].PropertyType = y.GetType().GetNestedPropertyType(this._fields[i].PropertyPath);
                            propertyType = this._fields[i].PropertyType;
                        }
                    }
                    object obj2 = this._fields[i].GetValue(x);
                    object obj3 = this._fields[i].GetValue(y);
                    IComparer comparer = this._fields[i].Comparer;
                    if ((propertyType != null) && (comparer == null))
                    {
                        this._fields[i].Comparer = typeof(Comparer<>).MakeGenericType(new Type[] { propertyType }).GetProperty("Default").GetValue(null, null) as IComparer;
                        comparer = this._fields[i].Comparer;
                    }
                    num = (comparer != null) ? comparer.Compare(obj2, obj3) : 0;
                    if (this._fields[i].Descending)
                    {
                        num = -num;
                    }
                    if (num != 0)
                    {
                        return num;
                    }
                }
                return num;
            }

            private static SortPropertyInfo[] CreatePropertyInfo(SortDescriptionCollection sortFields)
            {
                SortPropertyInfo[] infoArray = new SortPropertyInfo[sortFields.Count];
                for (int i = 0; i < sortFields.Count; i++)
                {
                    SortDescription description = sortFields[i];
                    infoArray[i].PropertyPath = description.PropertyName;
                    SortDescription description2 = sortFields[i];
                    infoArray[i].Descending = description2.Direction == ListSortDirection.Descending;
                }
                return infoArray;
            }

            public int FindInsertIndex(object x, IList list)
            {
                int num = 0;
                int num2 = list.Count - 1;
                while (num <= num2)
                {
                    int num3 = (num + num2) / 2;
                    int num4 = this.Compare(x, list[num3]);
                    if (num4 == 0)
                    {
                        return num3;
                    }
                    if (num4 > 0)
                    {
                        num = num3 + 1;
                    }
                    else
                    {
                        num2 = num3 - 1;
                    }
                }
                return num;
            }

            // Nested Types
            [StructLayout(LayoutKind.Sequential)]
            private struct SortPropertyInfo
            {
                internal IComparer Comparer;
                internal bool Descending;
                internal string PropertyPath;
                internal Type PropertyType;
                internal object GetValue(object o)
                {
                    if (string.IsNullOrEmpty(this.PropertyPath))
                    {
                        return ((this.PropertyType == o.GetType()) ? o : null);
                    }
                    return PagedCollectionView.InvokePath(o, this.PropertyPath, this.PropertyType);
                }
            }
        }
    }
}
