using System.Windows.Threading;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Diagnostics;

namespace System.Windows.Data
{
    public class CollectionView : DispatcherObject, ICollectionView, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
    {
        // Fields
        private ArrayList _changeLog;
        private CultureInfo _culture;
        private SimpleMonitor _currentChangedMonitor;
        private bool _currentElementWasRemovedOrReplaced;
        private object _currentItem;
        private int _currentPosition;
        private int _deferLevel;
        private DispatcherOperation _dispatcherOperation;
        private IndexedEnumerable _enumerableWrapper;
        private Predicate<object> _filter;
        private CollectionViewFlags _flags;
        private static object _newItemPlaceholder = new NamedObject("NewItemPlaceholder");
        private IEnumerable _sourceCollection;
        private int _timestamp;
        private object _vmData;
        internal const string CountPropertyName = "Count";
        internal const string CulturePropertyName = "Culture";
        internal const string CurrentItemPropertyName = "CurrentItem";
        internal const string CurrentPositionPropertyName = "CurrentPosition";
        internal const string IsCurrentAfterLastPropertyName = "IsCurrentAfterLast";
        internal const string IsCurrentBeforeFirstPropertyName = "IsCurrentBeforeFirst";
        internal const string IsEmptyPropertyName = "IsEmpty";
        private static readonly CurrentChangingEventArgs uncancelableCurrentChangingEventArgs = new CurrentChangingEventArgs(false);

        // Events
        protected event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler CurrentChanged;

        public event CurrentChangingEventHandler CurrentChanging;

        protected event PropertyChangedEventHandler PropertyChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged;

        // Methods
        public CollectionView(IEnumerable collection) : this(collection, 0)
        {
        }

        internal CollectionView(IEnumerable collection, bool shouldProcessCollectionChanged) : this(collection)
        {
            this.SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, shouldProcessCollectionChanged);
        }

        internal CollectionView(IEnumerable collection, int moveToFirst)
        {
            this._changeLog = new ArrayList();
            this._currentChangedMonitor = new SimpleMonitor();
            this._flags = CollectionViewFlags.NeedsRefresh | CollectionViewFlags.ShouldProcessCollectionChanged;
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if ((base.GetType() == typeof(CollectionView)) && TraceData.IsEnabled)
            {
                TraceData.Trace(TraceEventType.Warning, TraceData.CollectionViewIsUnsupported);
            }
            this._sourceCollection = collection;
            INotifyCollectionChanged changed = collection as INotifyCollectionChanged;
            if (changed != null)
            {
                changed.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
                this.SetFlag(CollectionViewFlags.IsDynamic, true);
            }
            object current = null;
            int num = -1;
            if (moveToFirst >= 0)
            {
                IEnumerator enumerator = collection.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    num = 0;
                }
            }
            this._currentItem = current;
            this._currentPosition = num;
            this.SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, this._currentPosition < 0);
            this.SetFlag(CollectionViewFlags.IsCurrentAfterLast, this._currentPosition < 0);
            this.SetFlag(CollectionViewFlags.CachedIsEmpty, this._currentPosition < 0);
        }

        private void _MoveCurrentToPosition(int position)
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
                this.SetCurrent(this.EnumerableWrapper[position], position);
            }
        }

        private void AdjustCurrencyForAdd(int index)
        {
            if (this.Count == 1)
            {
                this._currentPosition = -1;
            }
            else if (index <= this._currentPosition)
            {
                this._currentPosition++;
                if (this._currentPosition < this.Count)
                {
                    this._currentItem = this.EnumerableWrapper[this._currentPosition];
                }
            }
        }

        private void AdjustCurrencyForMove(int oldIndex, int newIndex)
        {
            if (((oldIndex >= this.CurrentPosition) || (newIndex >= this.CurrentPosition)) && ((oldIndex <= this.CurrentPosition) || (newIndex <= this.CurrentPosition)))
            {
                if (oldIndex <= this.CurrentPosition)
                {
                    this.AdjustCurrencyForRemove(oldIndex);
                }
                else if (newIndex <= this.CurrentPosition)
                {
                    this.AdjustCurrencyForAdd(newIndex);
                }
            }
        }

        private void AdjustCurrencyForRemove(int index)
        {
            if (index < this._currentPosition)
            {
                this._currentPosition--;
            }
            else if (index == this._currentPosition)
            {
                this._currentElementWasRemovedOrReplaced = true;
            }
        }

        private void AdjustCurrencyForReplace(int index)
        {
            if (index == this._currentPosition)
            {
                this._currentElementWasRemovedOrReplaced = true;
            }
        }

        private bool CheckFlag(CollectionViewFlags flags)
        {
            return ((this._flags & flags) != 0);
        }

        protected void ClearChangeLog()
        {
            lock (this._changeLog.SyncRoot)
            {
                this._changeLog.Clear();
            }
        }

        public virtual bool Contains(object item)
        {
            this.VerifyRefreshNotDeferred();
            return (this.IndexOf(item) >= 0);
        }

        private void DeferProcessing(ICollection changeLog)
        {
            if (changeLog == null)
            {
                changeLog = new object[0];
            }
            lock (this.SyncRoot)
            {
                lock (this._changeLog.SyncRoot)
                {
                    if (this._changeLog == null)
                    {
                        this._changeLog = new ArrayList(changeLog);
                    }
                    else
                    {
                        this._changeLog.InsertRange(0, changeLog);
                    }
                    if ((this._dispatcherOperation != null) && (this._dispatcherOperation.Priority == DispatcherPriority.DataBind))
                    {
                        this._dispatcherOperation.Priority = DispatcherPriority.Input;
                    }
                    else
                    {
                        this._dispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.ProcessInvoke), null);
                    }
                }
            }
        }

        public virtual IDisposable DeferRefresh()
        {
            IEditableCollectionView view = this as IEditableCollectionView;
            if ((view != null) && (view.IsAddingNew || view.IsEditingItem))
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "DeferRefresh" }));
            }
            this._deferLevel++;
            return new DeferHelper(this);
        }

        private void EndDefer()
        {
            this._deferLevel--;
            if ((this._deferLevel == 0) && this.CheckFlag(CollectionViewFlags.NeedsRefresh))
            {
                this.Refresh();
            }
        }

        protected virtual IEnumerator GetEnumerator()
        {
            this.VerifyRefreshNotDeferred();
            if (this.SortDescriptions.Count > 0)
            {
                throw new InvalidOperationException(SR.Get("ImplementOtherMembersWithSort", new object[] { "GetEnumerator()" }));
            }
            return this.EnumerableWrapper.GetEnumerator();
        }

        public virtual object GetItemAt(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return this.EnumerableWrapper[index];
        }

        internal ReadOnlyCollection<ItemPropertyInfo> GetItemProperties()
        {
            IEnumerable sourceCollection = this.SourceCollection;
            if (sourceCollection == null)
            {
                return null;
            }
            IEnumerable itemProperties = null;
            ITypedList list = sourceCollection as ITypedList;
            if (list != null)
            {
                itemProperties = list.GetItemProperties(null);
            }
            else
            {
                Type itemType = this.GetItemType(false);
                if (itemType != null)
                {
                    itemProperties = TypeDescriptor.GetProperties(itemType);
                }
                else
                {
                    object representativeItem = this.GetRepresentativeItem();
                    if (representativeItem != null)
                    {
                        itemProperties = TypeDescriptor.GetProperties(representativeItem);
                    }
                }
            }
            if (itemProperties == null)
            {
                return null;
            }
            List<ItemPropertyInfo> list2 = new List<ItemPropertyInfo>();
            foreach (object obj3 in itemProperties)
            {
                PropertyDescriptor descriptor = obj3 as PropertyDescriptor;
                if (descriptor != null)
                {
                    list2.Add(new ItemPropertyInfo(descriptor.Name, descriptor.PropertyType, descriptor));
                }
                else
                {
                    PropertyInfo info = obj3 as PropertyInfo;
                    if (info != null)
                    {
                        list2.Add(new ItemPropertyInfo(info.Name, info.PropertyType, info));
                    }
                }
            }
            return new ReadOnlyCollection<ItemPropertyInfo>(list2);
        }

        internal Type GetItemType(bool useRepresentativeItem)
        {
            Type type = this.SourceCollection.GetType();
            if (type.IsGenericType)
            {
                Type[] genericArguments = type.GetGenericArguments();
                if (genericArguments.Length == 1)
                {
                    return genericArguments[0];
                }
            }
            else if (useRepresentativeItem)
            {
                object representativeItem = this.GetRepresentativeItem();
                if (representativeItem != null)
                {
                    return representativeItem.GetType();
                }
            }
            return null;
        }

        internal object GetRepresentativeItem()
        {
            if (!this.IsEmpty)
            {
                int index = 0;
                IEditableCollectionView view = this as IEditableCollectionView;
                if ((view != null) && (view.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning))
                {
                    index = 1;
                }
                if (index < this.Count)
                {
                    return this.GetItemAt(index);
                }
            }
            return null;
        }

        internal virtual bool HasReliableHashCodes()
        {
            if (!this.IsEmpty)
            {
                return HashHelper.HasReliableHashCode(this.GetItemAt(0));
            }
            return true;
        }

        public virtual int IndexOf(object item)
        {
            this.VerifyRefreshNotDeferred();
            return this.EnumerableWrapper.IndexOf(item);
        }

        internal void InvalidateEnumerableWrapper()
        {
            IndexedEnumerable enumerable = Interlocked.Exchange<IndexedEnumerable>(ref this._enumerableWrapper, null);
            if (enumerable != null)
            {
                enumerable.Invalidate();
            }
        }

        private void MoveCurrencyOffDeletedElement()
        {
            int num = this.Count - 1;
            int position = (this._currentPosition < num) ? this._currentPosition : num;
            this.OnCurrentChanging();
            this._MoveCurrentToPosition(position);
            this.OnCurrentChanged();
        }

        public virtual bool MoveCurrentTo(object item)
        {
            this.VerifyRefreshNotDeferred();
            if ((object.Equals(this.CurrentItem, item) || object.Equals(NewItemPlaceholder, item)) && ((item != null) || this.IsCurrentInView))
            {
                return this.IsCurrentInView;
            }
            int position = -1;
            if (this.PassesFilter(item))
            {
                position = this.IndexOf(item);
            }
            return this.MoveCurrentToPosition(position);
        }

        public virtual bool MoveCurrentToFirst()
        {
            this.VerifyRefreshNotDeferred();
            int position = 0;
            IEditableCollectionView view = this as IEditableCollectionView;
            if ((view != null) && (view.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning))
            {
                position = 1;
            }
            return this.MoveCurrentToPosition(position);
        }

        public virtual bool MoveCurrentToLast()
        {
            this.VerifyRefreshNotDeferred();
            int position = this.Count - 1;
            IEditableCollectionView view = this as IEditableCollectionView;
            if ((view != null) && (view.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd))
            {
                position--;
            }
            return this.MoveCurrentToPosition(position);
        }

        public virtual bool MoveCurrentToNext()
        {
            this.VerifyRefreshNotDeferred();
            int position = this.CurrentPosition + 1;
            int count = this.Count;
            IEditableCollectionView view = this as IEditableCollectionView;
            if (((view != null) && (position == 0)) && (view.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning))
            {
                position = 1;
            }
            if (((view != null) && (position == (count - 1))) && (view.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd))
            {
                position = count;
            }
            return ((position <= count) && this.MoveCurrentToPosition(position));
        }

        public virtual bool MoveCurrentToPosition(int position)
        {
            this.VerifyRefreshNotDeferred();
            if ((position < -1) || (position > this.Count))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            IEditableCollectionView view = this as IEditableCollectionView;
            if (((view == null) || (((position != 0) || (view.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)) && ((position != (this.Count - 1)) || (view.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtEnd)))) && (((position != this.CurrentPosition) || !this.IsCurrentInSync) && this.OKToChangeCurrent()))
            {
                bool isCurrentAfterLast = this.IsCurrentAfterLast;
                bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                this._MoveCurrentToPosition(position);
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

        public virtual bool MoveCurrentToPrevious()
        {
            this.VerifyRefreshNotDeferred();
            int position = this.CurrentPosition - 1;
            int count = this.Count;
            IEditableCollectionView view = this as IEditableCollectionView;
            if (((view != null) && (position == (count - 1))) && (view.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd))
            {
                position = count - 2;
            }
            if (((view != null) && (position == 0)) && (view.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning))
            {
                position = -1;
            }
            return ((position >= -1) && this.MoveCurrentToPosition(position));
        }

        protected bool OKToChangeCurrent()
        {
            CurrentChangingEventArgs args = new CurrentChangingEventArgs();
            this.OnCurrentChanging(args);
            return !args.Cancel;
        }

        protected virtual void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
        {
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            this._timestamp++;
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, args);
            }
            if ((args.Action != NotifyCollectionChangedAction.Replace) && (args.Action != NotifyCollectionChangedAction.Move))
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

        protected void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (this.CheckFlag(CollectionViewFlags.ShouldProcessCollectionChanged))
            {
                bool flag = base.CheckAccess();
                if (!flag && !this.CheckFlag(CollectionViewFlags.IsMultiThreadCollectionChangeAllowed))
                {
                    throw new NotSupportedException(SR.Get("MultiThreadedCollectionChangeNotSupported"));
                }
                if (flag && !this.UpdatedOutsideDispatcher)
                {
                    this.ProcessCollectionChanged(args);
                }
                else
                {
                    this.PostChange(args);
                }
            }
        }

        protected virtual void OnCurrentChanged()
        {
            if ((this.CurrentChanged != null) && this._currentChangedMonitor.Enter())
            {
                using (this._currentChangedMonitor)
                {
                    this.CurrentChanged(this, EventArgs.Empty);
                }
            }
        }

        protected void OnCurrentChanging()
        {
            this._currentPosition = -1;
            this.OnCurrentChanging(uncancelableCurrentChangingEventArgs);
        }

        protected virtual void OnCurrentChanging(CurrentChangingEventArgs args)
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

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
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

        public virtual bool PassesFilter(object item)
        {
            if (this.CanFilter && (this.Filter != null))
            {
                return this.Filter(item);
            }
            return true;
        }

        private void PostChange(NotifyCollectionChangedEventArgs args)
        {
            lock (this.SyncRoot)
            {
                lock (this._changeLog.SyncRoot)
                {
                    this._changeLog.Add(args);
                    if (this._changeLog.Count <= 1)
                    {
                        this._dispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new DispatcherOperationCallback(this.ProcessInvoke), null);
                        if (!this.UpdatedOutsideDispatcher)
                        {
                            this.OnBeginChangeLogging(args);
                            this.SetFlag(CollectionViewFlags.UpdatedOutsideDispatcher, true);
                        }
                    }
                }
            }
        }

        private ICollection ProcessChangeLog(ArrayList changeLog)
        {
            int count = 0;
            bool flag = false;
            long ticks = DateTime.Now.Ticks;
            while ((count < changeLog.Count) && !flag)
            {
                NotifyCollectionChangedEventArgs args = changeLog[count] as NotifyCollectionChangedEventArgs;
                if (args != null)
                {
                    this.ProcessCollectionChanged(args);
                }
                flag = (DateTime.Now.Ticks - ticks) > 0xf4240L;
                count++;
            }
            if (flag)
            {
                changeLog.RemoveRange(0, count);
                return changeLog;
            }
            return null;
        }

        protected virtual void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            this.ValidateCollectionChangedEventArgs(args);
            object obj2 = this._currentItem;
            bool flag = this.CheckFlag(CollectionViewFlags.IsCurrentAfterLast);
            bool flag2 = this.CheckFlag(CollectionViewFlags.IsCurrentBeforeFirst);
            int num = this._currentPosition;
            bool flag3 = false;
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (this.PassesFilter(args.NewItems[0]))
                    {
                        flag3 = true;
                        this.AdjustCurrencyForAdd(args.NewStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (this.PassesFilter(args.OldItems[0]))
                    {
                        flag3 = true;
                        this.AdjustCurrencyForRemove(args.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (this.PassesFilter(args.OldItems[0]) || this.PassesFilter(args.NewItems[0]))
                    {
                        flag3 = true;
                        this.AdjustCurrencyForReplace(args.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (this.PassesFilter(args.NewItems[0]))
                    {
                        flag3 = true;
                        this.AdjustCurrencyForMove(args.OldStartingIndex, args.NewStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.RefreshOrDefer();
                    return;
            }
            if (flag3)
            {
                this.OnCollectionChanged(args);
            }
            if (this._currentElementWasRemovedOrReplaced)
            {
                this.MoveCurrencyOffDeletedElement();
                this._currentElementWasRemovedOrReplaced = false;
            }
            if (this.IsCurrentAfterLast != flag)
            {
                this.OnPropertyChanged("IsCurrentAfterLast");
            }
            if (this.IsCurrentBeforeFirst != flag2)
            {
                this.OnPropertyChanged("IsCurrentBeforeFirst");
            }
            if (this._currentPosition != num)
            {
                this.OnPropertyChanged("CurrentPosition");
            }
            if (this._currentItem != obj2)
            {
                this.OnPropertyChanged("CurrentItem");
            }
        }

        private object ProcessInvoke(object arg)
        {
            ArrayList list;
            lock (this.SyncRoot)
            {
                lock (this._changeLog.SyncRoot)
                {
                    this._dispatcherOperation = null;
                    list = this._changeLog;
                    this._changeLog = new ArrayList();
                }
            }
            foreach (NotifyCollectionChangedEventArgs args in list)
            {
                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    list = new ArrayList(1);
                    list.Add(args);
                    break;
                }
            }
            ICollection changeLog = this.ProcessChangeLog(list);
            if ((changeLog != null) && (changeLog.Count > 0))
            {
                this.DeferProcessing(changeLog);
            }
            return null;
        }

        public virtual void Refresh()
        {
            IEditableCollectionView view = this as IEditableCollectionView;
            if ((view != null) && (view.IsAddingNew || view.IsEditingItem))
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "Refresh" }));
            }
            this.RefreshInternal();
        }

        internal void RefreshInternal()
        {
            this.RefreshOverride();
            this.SetFlag(CollectionViewFlags.NeedsRefresh, false);
        }

        protected void RefreshOrDefer()
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

        protected virtual void RefreshOverride()
        {
            if (this.SortDescriptions.Count > 0)
            {
                throw new InvalidOperationException(SR.Get("ImplementOtherMembersWithSort", new object[] { "Refresh()" }));
            }
            object item = this._currentItem;
            bool flag = this.CheckFlag(CollectionViewFlags.IsCurrentAfterLast);
            bool flag2 = this.CheckFlag(CollectionViewFlags.IsCurrentBeforeFirst);
            int num = this._currentPosition;
            this.OnCurrentChanging();
            this.InvalidateEnumerableWrapper();
            if (this.IsEmpty || flag2)
            {
                this._MoveCurrentToPosition(-1);
            }
            else if (flag)
            {
                this._MoveCurrentToPosition(this.Count);
            }
            else if (item != null)
            {
                int index = this.EnumerableWrapper.IndexOf(item);
                if (index < 0)
                {
                    index = 0;
                }
                this._MoveCurrentToPosition(index);
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnCurrentChanged();
            if (this.IsCurrentAfterLast != flag)
            {
                this.OnPropertyChanged("IsCurrentAfterLast");
            }
            if (this.IsCurrentBeforeFirst != flag2)
            {
                this.OnPropertyChanged("IsCurrentBeforeFirst");
            }
            if (num != this.CurrentPosition)
            {
                this.OnPropertyChanged("CurrentPosition");
            }
            if (item != this.CurrentItem)
            {
                this.OnPropertyChanged("CurrentItem");
            }
        }

        protected void SetCurrent(object newItem, int newPosition)
        {
            int count = (newItem != null) ? 0 : (this.IsEmpty ? 0 : this.Count);
            this.SetCurrent(newItem, newPosition, count);
        }

        protected void SetCurrent(object newItem, int newPosition, int count)
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

        internal void SetViewManagerData(object value)
        {
            if (this._vmData == null)
            {
                this._vmData = value;
            }
            else
            {
                object[] objArray = this._vmData as object[];
                if (objArray == null)
                {
                    this._vmData = new object[] { this._vmData, value };
                }
                else
                {
                    object[] array = new object[objArray.Length + 1];
                    objArray.CopyTo(array, 0);
                    array[objArray.Length] = value;
                    this._vmData = array;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems.Count != 1)
                    {
                        throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Count != 1)
                    {
                        throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
                    }
                    if (e.OldStartingIndex >= 0)
                    {
                        break;
                    }
                    throw new InvalidOperationException(SR.Get("RemovedItemNotFound"));

                case NotifyCollectionChangedAction.Replace:
                    if ((e.NewItems.Count != 1) || (e.OldItems.Count != 1))
                    {
                        throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.NewItems.Count != 1)
                    {
                        throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
                    }
                    if (e.NewStartingIndex >= 0)
                    {
                        break;
                    }
                    throw new InvalidOperationException(SR.Get("CannotMoveToUnknownPosition"));

                case NotifyCollectionChangedAction.Reset:
                    break;

                default:
                    throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[] { e.Action }));
            }
        }

        internal void VerifyRefreshNotDeferred()
        {
            if (this.IsRefreshDeferred)
            {
                throw new InvalidOperationException(SR.Get("NoCheckOrChangeWhenDeferred"));
            }
        }

        // Properties
        public virtual bool CanFilter
        {
            get
            {
                return true;
            }
        }

        public virtual bool CanGroup
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanSort
        {
            get
            {
                return false;
            }
        }

        public virtual IComparer Comparer
        {
            get
            {
                return (this as IComparer);
            }
        }

        public virtual int Count
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this.EnumerableWrapper.Count;
            }
        }

        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public virtual CultureInfo Culture
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

        public virtual object CurrentItem
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this._currentItem;
            }
        }

        public virtual int CurrentPosition
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this._currentPosition;
            }
        }

        private IndexedEnumerable EnumerableWrapper
        {
            get
            {
                if (this._enumerableWrapper == null)
                {
                    IndexedEnumerable enumerable = new IndexedEnumerable(this.SourceCollection, new Predicate<object>(this.PassesFilter));
                    Interlocked.CompareExchange<IndexedEnumerable>(ref this._enumerableWrapper, enumerable, null);
                }
                return this._enumerableWrapper;
            }
        }

        public virtual Predicate<object> Filter
        {
            get
            {
                return this._filter;
            }
            set
            {
                if (!this.CanFilter)
                {
                    throw new NotSupportedException();
                }
                this._filter = value;
                this.RefreshOrDefer();
            }
        }

        public virtual ObservableCollection<GroupDescription> GroupDescriptions
        {
            get
            {
                return null;
            }
        }

        public virtual ReadOnlyObservableCollection<object> Groups
        {
            get
            {
                return null;
            }
        }

        public virtual bool IsCurrentAfterLast
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this.CheckFlag(CollectionViewFlags.IsCurrentAfterLast);
            }
        }

        public virtual bool IsCurrentBeforeFirst
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return this.CheckFlag(CollectionViewFlags.IsCurrentBeforeFirst);
            }
        }

        protected bool IsCurrentInSync
        {
            get
            {
                if (this.IsCurrentInView)
                {
                    return (this.GetItemAt(this.CurrentPosition) == this.CurrentItem);
                }
                return (this.CurrentItem == null);
            }
        }

        private bool IsCurrentInView
        {
            get
            {
                this.VerifyRefreshNotDeferred();
                return ((0 <= this.CurrentPosition) && (this.CurrentPosition < this.Count));
            }
        }

        protected bool IsDynamic
        {
            get
            {
                return this.CheckFlag(CollectionViewFlags.IsDynamic);
            }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return this.EnumerableWrapper.IsEmpty;
            }
        }

        protected bool IsRefreshDeferred
        {
            get
            {
                return (this._deferLevel > 0);
            }
        }

        public virtual bool NeedsRefresh
        {
            get
            {
                return this.CheckFlag(CollectionViewFlags.NeedsRefresh);
            }
        }

        public static object NewItemPlaceholder
        {
            get
            {
                return _newItemPlaceholder;
            }
        }

        public virtual SortDescriptionCollection SortDescriptions
        {
            get
            {
                return SortDescriptionCollection.Empty;
            }
        }

        public virtual IEnumerable SourceCollection
        {
            get
            {
                return this._sourceCollection;
            }
        }

        internal object SyncRoot
        {
            get
            {
                ICollection sourceCollection = this.SourceCollection as ICollection;
                if (sourceCollection != null)
                {
                    return sourceCollection.SyncRoot;
                }
                return this.SourceCollection;
            }
        }

        internal int Timestamp
        {
            get
            {
                return this._timestamp;
            }
        }

        protected bool UpdatedOutsideDispatcher
        {
            get
            {
                return this.CheckFlag(CollectionViewFlags.UpdatedOutsideDispatcher);
            }
        }

        // Nested Types
        [Flags]
        private enum CollectionViewFlags
        {
            CachedIsEmpty = 0x200,
            IsCurrentAfterLast = 0x10,
            IsCurrentBeforeFirst = 8,
            IsDataInGroupOrder = 0x40,
            IsDynamic = 0x20,
            IsMultiThreadCollectionChangeAllowed = 0x100,
            NeedsRefresh = 0x80,
            ShouldProcessCollectionChanged = 4,
            UpdatedOutsideDispatcher = 2
        }

        private class DeferHelper : IDisposable
        {
            // Fields
            private CollectionView _collectionView;

            // Methods
            public DeferHelper(CollectionView collectionView)
            {
                this._collectionView = collectionView;
            }

            public void Dispose()
            {
                if (this._collectionView != null)
                {
                    this._collectionView.EndDefer();
                    this._collectionView = null;
                }
            }
        }

        internal class PlaceholderAwareEnumerator : IEnumerator
        {
            // Fields
            private IEnumerator _baseEnumerator;
            private CollectionView _collectionView;
            private object _newItem;
            private NewItemPlaceholderPosition _placeholderPosition;
            private Position _position;
            private int _timestamp;

            // Methods
            public PlaceholderAwareEnumerator(CollectionView collectionView, IEnumerator baseEnumerator, NewItemPlaceholderPosition placeholderPosition, object newItem)
            {
                this._collectionView = collectionView;
                this._timestamp = collectionView.Timestamp;
                this._baseEnumerator = baseEnumerator;
                this._placeholderPosition = placeholderPosition;
                this._newItem = newItem;
            }

            public bool MoveNext()
            {
                if (this._timestamp != this._collectionView.Timestamp)
                {
                    throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
                }
                switch (this._position)
                {
                    case Position.BeforePlaceholder:
                        if (this._placeholderPosition != NewItemPlaceholderPosition.AtBeginning)
                        {
                            if (!this._baseEnumerator.MoveNext() || (((this._newItem != null) && (this._baseEnumerator.Current == this._newItem)) && !this._baseEnumerator.MoveNext()))
                            {
                                if (this._newItem != null)
                                {
                                    this._position = Position.OnNewItem;
                                }
                                else
                                {
                                    if (this._placeholderPosition == NewItemPlaceholderPosition.None)
                                    {
                                        return false;
                                    }
                                    this._position = Position.OnPlaceholder;
                                }
                            }
                            break;
                        }
                        this._position = Position.OnPlaceholder;
                        break;

                    case Position.OnPlaceholder:
                        if ((this._newItem == null) || (this._placeholderPosition != NewItemPlaceholderPosition.AtBeginning))
                        {
                            goto Label_00D8;
                        }
                        this._position = Position.OnNewItem;
                        return true;

                    case Position.OnNewItem:
                        if (this._placeholderPosition != NewItemPlaceholderPosition.AtEnd)
                        {
                            goto Label_00D8;
                        }
                        this._position = Position.OnPlaceholder;
                        return true;

                    default:
                        goto Label_00D8;
                }
                return true;
            Label_00D8:
                this._position = Position.AfterPlaceholder;
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
                this._position = Position.BeforePlaceholder;
                this._baseEnumerator.Reset();
            }

            // Properties
            public object Current
            {
                get
                {
                    if (this._position == Position.OnPlaceholder)
                    {
                        return CollectionView.NewItemPlaceholder;
                    }
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
                BeforePlaceholder,
                OnPlaceholder,
                OnNewItem,
                AfterPlaceholder
            }
        }

        private class SimpleMonitor : IDisposable
        {
            // Fields
            private bool _entered;

            // Methods
            public void Dispose()
            {
                this._entered = false;
            }

            public bool Enter()
            {
                if (this._entered)
                {
                    return false;
                }
                this._entered = true;
                return true;
            }

            // Properties
            public bool Busy
            {
                get
                {
                    return this._entered;
                }
            }
        }
    }
}