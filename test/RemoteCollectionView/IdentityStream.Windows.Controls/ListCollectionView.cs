using System.Windows.Data;
using System;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Windows.Data
{
    public class ListCollectionView : CollectionView, IComparer, IEditableCollectionView, IItemProperties
    {
        // Fields
        private IComparer _activeComparer;
        private Predicate<object> _activeFilter;
        private bool _applyChangeToShadow;
        private bool _currentElementWasRemoved;
        private IComparer _customSort;
        private object _editItem;
        private CollectionViewGroupRoot _group;
        private IList _internalList;
        private bool _isGrouping;
        private bool _isItemConstructorValid;
        private ConstructorInfo _itemConstructor;
        private object _newItem;
        private int _newItemIndex;
        private NewItemPlaceholderPosition _newItemPlaceholderPosition;
        private ArrayList _shadowCollection;
        private SortDescriptionCollection _sort;
        private const int _unknownIndex = -1;

        // Methods
        public ListCollectionView(IList list)
            : base(list)
        {
            this._internalList = list;
            if (this.InternalList.Count == 0)
            {
                base.SetCurrent(null, -1, 0);
            }
            else
            {
                base.SetCurrent(this.InternalList[0], 0, 1);
            }
            this._group = new CollectionViewGroupRoot(this);
            this._group.GroupDescriptionChanged += new EventHandler(this.OnGroupDescriptionChanged);
            this._group.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnGroupChanged);
            this._group.GroupDescriptions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnGroupByChanged);
        }

        private void AddItemToGroups(object item)
        {
            if (!this.IsAddingNew || (item != this._newItem))
            {
                this._group.AddToSubgroups(item, false);
            }
            else
            {
                int count;
                switch (this.NewItemPlaceholderPosition)
                {
                    case NewItemPlaceholderPosition.AtBeginning:
                        count = 1;
                        break;

                    case NewItemPlaceholderPosition.AtEnd:
                        count = this._group.Items.Count - 1;
                        break;

                    default:
                        count = this._group.Items.Count;
                        break;
                }
                this._group.InsertSpecialItem(count, item, false);
            }
        }

        public object AddNew()
        {
            base.VerifyRefreshNotDeferred();
            if (this.IsEditingItem)
            {
                this.CommitEdit();
            }
            this.CommitNew();
            if (!this.CanAddNew)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[] { "AddNew" }));
            }
            object obj2 = this._itemConstructor.Invoke(null);
            this._newItemIndex = -2;
            int index = this.SourceList.Add(obj2);
            if (!(this.SourceList is INotifyCollectionChanged))
            {
                if (!object.Equals(obj2, this.SourceList[index]))
                {
                    index = this.SourceList.IndexOf(obj2);
                }
                this.BeginAddNew(obj2, index);
            }
            this.MoveCurrentTo(obj2);
            ISupportInitialize initialize = obj2 as ISupportInitialize;
            if (initialize != null)
            {
                initialize.BeginInit();
            }
            IEditableObject obj3 = obj2 as IEditableObject;
            if (obj3 != null)
            {
                obj3.BeginEdit();
            }
            return obj2;
        }

        private int AdjustBefore(NotifyCollectionChangedAction action, object item, int index)
        {
            if (action == NotifyCollectionChangedAction.Reset)
            {
                return -1;
            }
            if (item == CollectionView.NewItemPlaceholder)
            {
                if (this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
                {
                    return (this.InternalCount - 1);
                }
                return 0;
            }
            if ((this.IsAddingNew && (this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None)) && object.Equals(item, this._newItem))
            {
                if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
                {
                    return 1;
                }
                if (!this.UsesLocalArray)
                {
                    return index;
                }
                return (this.InternalCount - 2);
            }
            int num = this.IsGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? (this.IsAddingNew ? 2 : 1) : 0);
            IList ilFull = (base.UpdatedOutsideDispatcher ? this.ShadowCollection : ((IList)this.SourceCollection)) as IList;
            if ((index < -1) || (index > ilFull.Count))
            {
                throw new InvalidOperationException(SR.Get("CollectionChangeIndexOutOfRange", new object[] { index, ilFull.Count }));
            }
            if (action == NotifyCollectionChangedAction.Add)
            {
                if (index >= 0)
                {
                    if (!object.Equals(item, ilFull[index]))
                    {
                        throw new InvalidOperationException(SR.Get("AddedItemNotAtIndex", new object[] { index }));
                    }
                }
                else
                {
                    index = ilFull.IndexOf(item);
                    if (index < 0)
                    {
                        throw new InvalidOperationException(SR.Get("AddedItemNotInCollection"));
                    }
                }
            }
            if (!this.UsesLocalArray)
            {
                if (this.IsAddingNew && (index > this._newItemIndex))
                {
                    index--;
                }
                return (index + num);
            }
            if (action == NotifyCollectionChangedAction.Add)
            {
                if (!this.PassesFilter(item))
                {
                    return -2;
                }
                IComparer comparer = (this.ActiveComparer != null) ? this.ActiveComparer : new ListOrdinalComparer(ilFull, item, index);
                ArrayList internalList = this.InternalList as ArrayList;
                if (internalList != null)
                {
                    index = internalList.BinarySearch(item, comparer);
                    if (index < 0)
                    {
                        index = ~index;
                    }
                }
                else
                {
                    index = -1;
                }
            }
            else if (action == NotifyCollectionChangedAction.Remove)
            {
                if (!this.IsAddingNew || (item != this._newItem))
                {
                    index = this.InternalList.IndexOf(item);
                    if (index < 0)
                    {
                        return -2;
                    }
                }
                else
                {
                    switch (this.NewItemPlaceholderPosition)
                    {
                        case NewItemPlaceholderPosition.None:
                            return (this.InternalCount - 1);

                        case NewItemPlaceholderPosition.AtBeginning:
                            return 1;

                        case NewItemPlaceholderPosition.AtEnd:
                            return (this.InternalCount - 2);
                    }
                }
            }
            else
            {
                index = -1;
            }
            if (index >= 0)
            {
                return (index + num);
            }
            return index;
        }

        private void AdjustCurrencyForAdd(int index)
        {
            if (this.InternalCount == 1)
            {
                base.SetCurrent(null, -1);
            }
            else if (index <= this.CurrentPosition)
            {
                int newPosition = this.CurrentPosition + 1;
                if (newPosition < this.InternalCount)
                {
                    base.SetCurrent(this.GetItemAt(newPosition), newPosition);
                }
                else
                {
                    base.SetCurrent(null, this.InternalCount);
                }
            }
        }

        private void AdjustCurrencyForMove(int oldIndex, int newIndex)
        {
            if (oldIndex == this.CurrentPosition)
            {
                base.SetCurrent(this.GetItemAt(newIndex), newIndex);
            }
            else if ((oldIndex < this.CurrentPosition) && (this.CurrentPosition <= newIndex))
            {
                base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
            }
            else if ((newIndex <= this.CurrentPosition) && (this.CurrentPosition < oldIndex))
            {
                base.SetCurrent(this.CurrentItem, this.CurrentPosition + 1);
            }
        }

        private void AdjustCurrencyForRemove(int index)
        {
            if (index < this.CurrentPosition)
            {
                base.SetCurrent(this.CurrentItem, this.CurrentPosition - 1);
            }
            else if (index == this.CurrentPosition)
            {
                this._currentElementWasRemoved = true;
            }
        }

        private void AdjustCurrencyForReplace(int index)
        {
            if (index == this.CurrentPosition)
            {
                this._currentElementWasRemoved = true;
            }
        }

        internal void AdjustShadowCopy(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex <= -1)
                    {
                        this.ShadowCollection.Add(e.NewItems[0]);
                        return;
                    }
                    this.ShadowCollection.Insert(e.NewStartingIndex, e.NewItems[0]);
                    return;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex <= -1)
                    {
                        this.ShadowCollection.Remove(e.OldItems[0]);
                        return;
                    }
                    this.ShadowCollection.RemoveAt(e.OldStartingIndex);
                    return;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex <= -1)
                    {
                        this.ShadowCollection.IndexOf(e.OldItems[0]);
                        this.ShadowCollection[e.OldStartingIndex] = e.NewItems[0];
                        return;
                    }
                    this.ShadowCollection[e.OldStartingIndex] = e.NewItems[0];
                    return;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex <= -1)
                    {
                        this.ShadowCollection.Remove(e.OldItems[0]);
                        this.ShadowCollection.Insert(e.NewStartingIndex, e.NewItems[0]);
                        return;
                    }
                    this.ShadowCollection.RemoveAt(e.OldStartingIndex);
                    return;
            }
            throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[] { e.Action }));
        }

        private void BeginAddNew(object newItem, int index)
        {
            this._newItem = newItem;
            this._newItemIndex = index;
            int adjustedNewIndex = -1;
            switch (this.NewItemPlaceholderPosition)
            {
                case NewItemPlaceholderPosition.None:
                    adjustedNewIndex = this.UsesLocalArray ? (this.InternalCount - 1) : this._newItemIndex;
                    break;

                case NewItemPlaceholderPosition.AtBeginning:
                    adjustedNewIndex = 1;
                    break;

                case NewItemPlaceholderPosition.AtEnd:
                    adjustedNewIndex = this.InternalCount - 2;
                    break;
            }
            this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, adjustedNewIndex), -1, adjustedNewIndex);
        }

        public void CancelEdit()
        {
            if (this.IsAddingNew)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[] { "CancelEdit", "AddNew" }));
            }
            base.VerifyRefreshNotDeferred();
            if (this._editItem != null)
            {
                IEditableObject obj2 = this._editItem as IEditableObject;
                this._editItem = null;
                if (obj2 == null)
                {
                    throw new InvalidOperationException(SR.Get("CancelEditNotSupported"));
                }
                obj2.CancelEdit();
            }
        }

        public void CancelNew()
        {
            if (this.IsEditingItem)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[] { "CancelNew", "EditItem" }));
            }
            base.VerifyRefreshNotDeferred();
            if (this._newItem != null)
            {
                this.SourceList.RemoveAt(this._newItemIndex);
                if (this._newItem != null)
                {
                    int adjustedOldIndex = this.AdjustBefore(NotifyCollectionChangedAction.Remove, this._newItem, this._newItemIndex);
                    object changedItem = this.EndAddNew(true);
                    this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, adjustedOldIndex), adjustedOldIndex, -1);
                }
            }
        }

        public void CommitEdit()
        {
            if (this.IsAddingNew)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[] { "CommitEdit", "AddNew" }));
            }
            base.VerifyRefreshNotDeferred();
            if (this._editItem != null)
            {
                object item = this._editItem;
                IEditableObject obj3 = this._editItem as IEditableObject;
                this._editItem = null;
                if (obj3 != null)
                {
                    obj3.EndEdit();
                }
                if (this.IsGrouping)
                {
                    this.RemoveItemFromGroups(item);
                    this.AddItemToGroups(item);
                }
                else if (this.UsesLocalArray)
                {
                    int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
                    int adjustedOldIndex = this.InternalIndexOf(item);
                    if (!this.PassesFilter(item))
                    {
                        this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, adjustedOldIndex), adjustedOldIndex, -1);
                    }
                    else if (this.ActiveComparer != null)
                    {
                        ArrayList internalList = (ArrayList)this.InternalList;
                        int count = adjustedOldIndex - num;
                        int num4 = -1;
                        if ((count > 0) && (this.ActiveComparer.Compare(internalList[count - 1], item) > 0))
                        {
                            num4 = internalList.BinarySearch(0, count, item, this.ActiveComparer);
                            if (num4 < 0)
                            {
                                num4 = ~num4;
                            }
                        }
                        else if ((count < (internalList.Count - 1)) && (this.ActiveComparer.Compare(item, internalList[count + 1]) > 0))
                        {
                            num4 = internalList.BinarySearch(count + 1, (internalList.Count - count) - 1, item, this.ActiveComparer);
                            if (num4 < 0)
                            {
                                num4 = ~num4;
                            }
                            num4--;
                        }
                        if (num4 >= 0)
                        {
                            this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, num4 + num, adjustedOldIndex), adjustedOldIndex, num4 + num);
                        }
                    }
                }
            }
        }

        public void CommitNew()
        {
            if (this.IsEditingItem)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[] { "CommitNew", "EditItem" }));
            }
            base.VerifyRefreshNotDeferred();
            if (this._newItem != null)
            {
                if (this.IsGrouping)
                {
                    this.CommitNewForGrouping();
                }
                else
                {
                    int adjustedOldIndex = 0;
                    switch (this.NewItemPlaceholderPosition)
                    {
                        case NewItemPlaceholderPosition.None:
                            adjustedOldIndex = this.UsesLocalArray ? (this.InternalCount - 1) : this._newItemIndex;
                            break;

                        case NewItemPlaceholderPosition.AtBeginning:
                            adjustedOldIndex = 1;
                            break;

                        case NewItemPlaceholderPosition.AtEnd:
                            adjustedOldIndex = this.InternalCount - 2;
                            break;
                    }
                    object item = this.EndAddNew(false);
                    int index = this.AdjustBefore(NotifyCollectionChangedAction.Add, item, this._newItemIndex);
                    if (index < 0)
                    {
                        this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, adjustedOldIndex), adjustedOldIndex, -1);
                    }
                    else if (adjustedOldIndex == index)
                    {
                        if (this.UsesLocalArray)
                        {
                            if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
                            {
                                index--;
                            }
                            this.InternalList.Insert(index, item);
                        }
                    }
                    else
                    {
                        this.ProcessCollectionChangedWithAdjustedIndex(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, index, adjustedOldIndex), adjustedOldIndex, index);
                    }
                }
            }
        }

        private void CommitNewForGrouping()
        {
            int num;
            switch (this.NewItemPlaceholderPosition)
            {
                case NewItemPlaceholderPosition.AtBeginning:
                    num = 1;
                    break;

                case NewItemPlaceholderPosition.AtEnd:
                    num = this._group.Items.Count - 2;
                    break;

                default:
                    num = this._group.Items.Count - 1;
                    break;
            }
            int index = this._newItemIndex;
            object item = this.EndAddNew(false);
            this._group.RemoveSpecialItem(num, item, false);
            this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected virtual int Compare(object o1, object o2)
        {
            if (!this.IsGrouping)
            {
                if (this.ActiveComparer != null)
                {
                    return this.ActiveComparer.Compare(o1, o2);
                }
                int index = this.InternalList.IndexOf(o1);
                int num2 = this.InternalList.IndexOf(o2);
                return (index - num2);
            }
            int num3 = this.InternalIndexOf(o1);
            int num4 = this.InternalIndexOf(o2);
            return (num3 - num4);
        }

        public override bool Contains(object item)
        {
            base.VerifyRefreshNotDeferred();
            return this.InternalContains(item);
        }

        public void EditItem(object item)
        {
            base.VerifyRefreshNotDeferred();
            if (item == CollectionView.NewItemPlaceholder)
            {
                throw new ArgumentException(SR.Get("CannotEditPlaceholder"), "item");
            }
            if (this.IsAddingNew)
            {
                if (object.Equals(item, this._newItem))
                {
                    return;
                }
                this.CommitNew();
            }
            this.CommitEdit();
            this._editItem = item;
            IEditableObject obj2 = item as IEditableObject;
            if (obj2 != null)
            {
                obj2.BeginEdit();
            }
        }

        private object EndAddNew(bool cancel)
        {
            object obj2 = this._newItem;
            this._newItem = null;
            IEditableObject obj3 = obj2 as IEditableObject;
            if (obj3 != null)
            {
                if (cancel)
                {
                    obj3.CancelEdit();
                }
                else
                {
                    obj3.EndEdit();
                }
            }
            ISupportInitialize initialize = obj2 as ISupportInitialize;
            if (initialize != null)
            {
                initialize.EndInit();
            }
            return obj2;
        }

        private void EnsureItemConstructor()
        {
            if (!this._isItemConstructorValid)
            {
                Type itemType = base.GetItemType(true);
                if (itemType != null)
                {
                    this._itemConstructor = itemType.GetConstructor(Type.EmptyTypes);
                    this._isItemConstructorValid = true;
                }
            }
        }

        protected override IEnumerator GetEnumerator()
        {
            base.VerifyRefreshNotDeferred();
            return this.InternalGetEnumerator();
        }

        public override object GetItemAt(int index)
        {
            base.VerifyRefreshNotDeferred();
            return this.InternalItemAt(index);
        }

        private void ImplicitlyCancelEdit()
        {
            IEditableObject obj2 = this._editItem as IEditableObject;
            this._editItem = null;
            if (obj2 != null)
            {
                obj2.CancelEdit();
            }
        }

        public override int IndexOf(object item)
        {
            base.VerifyRefreshNotDeferred();
            return this.InternalIndexOf(item);
        }

        protected bool InternalContains(object item)
        {
            if (item == CollectionView.NewItemPlaceholder)
            {
                return (this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None);
            }
            if (this.IsGrouping)
            {
                return (this._group.LeafIndexOf(item) >= 0);
            }
            return this.InternalList.Contains(item);
        }

        protected IEnumerator InternalGetEnumerator()
        {
            if (!this.IsGrouping)
            {
                return new CollectionView.PlaceholderAwareEnumerator(this, this.InternalList.GetEnumerator(), this.NewItemPlaceholderPosition, this._newItem);
            }
            return this._group.GetLeafEnumerator();
        }

        protected int InternalIndexOf(object item)
        {
            int num;
            if (this.IsGrouping)
            {
                return this._group.LeafIndexOf(item);
            }
            if (item == CollectionView.NewItemPlaceholder)
            {
                switch (this.NewItemPlaceholderPosition)
                {
                    case NewItemPlaceholderPosition.None:
                        return -1;

                    case NewItemPlaceholderPosition.AtBeginning:
                        return 0;

                    case NewItemPlaceholderPosition.AtEnd:
                        return (this.InternalCount - 1);
                }
            }
            else if (this.IsAddingNew && object.Equals(item, this._newItem))
            {
                switch (this.NewItemPlaceholderPosition)
                {
                    case NewItemPlaceholderPosition.None:
                        if (!this.UsesLocalArray)
                        {
                            goto Label_0092;
                        }
                        return (this.InternalCount - 1);

                    case NewItemPlaceholderPosition.AtBeginning:
                        return 1;

                    case NewItemPlaceholderPosition.AtEnd:
                        return (this.InternalCount - 2);
                }
            }
        Label_0092:
            num = this.InternalList.IndexOf(item);
            if ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) && (num >= 0))
            {
                num += this.IsAddingNew ? 2 : 1;
            }
            return num;
        }

        protected object InternalItemAt(int index)
        {
            if (this.IsGrouping)
            {
                return this._group.LeafAt(index);
            }
            switch (this.NewItemPlaceholderPosition)
            {
                case NewItemPlaceholderPosition.None:
                    if ((!this.IsAddingNew || !this.UsesLocalArray) || (index != (this.InternalCount - 1)))
                    {
                        break;
                    }
                    return this._newItem;

                case NewItemPlaceholderPosition.AtBeginning:
                    if (index != 0)
                    {
                        index--;
                        if (this.IsAddingNew)
                        {
                            if (index == 0)
                            {
                                return this._newItem;
                            }
                            if (this.UsesLocalArray || (index <= this._newItemIndex))
                            {
                                index--;
                            }
                        }
                        break;
                    }
                    return CollectionView.NewItemPlaceholder;

                case NewItemPlaceholderPosition.AtEnd:
                    if (index != (this.InternalCount - 1))
                    {
                        if (this.IsAddingNew)
                        {
                            if (index == (this.InternalCount - 2))
                            {
                                return this._newItem;
                            }
                            if (!this.UsesLocalArray && (index >= this._newItemIndex))
                            {
                                index++;
                            }
                        }
                        break;
                    }
                    return CollectionView.NewItemPlaceholder;
            }
            return this.InternalList[index];
        }

        private void MoveCurrencyOffDeletedElement(int oldCurrentPosition)
        {
            int num = this.InternalCount - 1;
            int newPosition = (oldCurrentPosition < num) ? oldCurrentPosition : num;
            this._currentElementWasRemoved = false;
            base.OnCurrentChanging();
            if (newPosition < 0)
            {
                base.SetCurrent(null, newPosition);
            }
            else
            {
                base.SetCurrent(this.InternalItemAt(newPosition), newPosition);
            }
            this.OnCurrentChanged();
        }

        public override bool MoveCurrentToPosition(int position)
        {
            base.VerifyRefreshNotDeferred();
            if ((position < -1) || (position > this.InternalCount))
            {
                throw new ArgumentOutOfRangeException("position");
            }
            if ((position != this.CurrentPosition) || !base.IsCurrentInSync)
            {
                object newItem = ((0 <= position) && (position < this.InternalCount)) ? this.InternalItemAt(position) : null;
                if ((newItem != CollectionView.NewItemPlaceholder) && base.OKToChangeCurrent())
                {
                    bool isCurrentAfterLast = this.IsCurrentAfterLast;
                    bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                    base.SetCurrent(newItem, position);
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
            }
            return this.IsCurrentInView;
        }

        protected override void OnBeginChangeLogging(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if ((this.ShadowCollection == null) || (args.Action == NotifyCollectionChangedAction.Reset))
            {
                this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
                if (!this.UsesLocalArray)
                {
                    this._internalList = this.ShadowCollection;
                }
                this._applyChangeToShadow = false;
            }
        }

        private void OnGroupByChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.IsAddingNew || this.IsEditingItem)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "Grouping" }));
            }
            base.RefreshOrDefer();
        }

        private void OnGroupChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                this.AdjustCurrencyForAdd(e.NewStartingIndex);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                this.AdjustCurrencyForRemove(e.OldStartingIndex);
            }
            this.OnCollectionChanged(e);
        }

        private void OnGroupDescriptionChanged(object sender, EventArgs e)
        {
            if (this.IsAddingNew || this.IsEditingItem)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "Grouping" }));
            }
            base.RefreshOrDefer();
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public override bool PassesFilter(object item)
        {
            if (this.ActiveFilter != null)
            {
                return this.ActiveFilter(item);
            }
            return true;
        }

        private void PrepareGroups()
        {
            this._group.Clear();
            this._group.Initialize();
            this._isGrouping = this._group.GroupBy != null;
            if (this._isGrouping)
            {
                if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
                {
                    this._group.InsertSpecialItem(0, CollectionView.NewItemPlaceholder, true);
                    if (this.IsAddingNew)
                    {
                        this._group.InsertSpecialItem(1, this._newItem, true);
                    }
                }
                int num = 0;
                int count = this.InternalList.Count;
                while (num < count)
                {
                    object objB = this.InternalList[num];
                    if (!this.IsAddingNew || !object.Equals(this._newItem, objB))
                    {
                        this._group.AddToSubgroups(objB, true);
                    }
                    num++;
                }
                if (this.IsAddingNew && (this.NewItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning))
                {
                    this._group.InsertSpecialItem(this._group.Items.Count, this._newItem, true);
                }
                if (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
                {
                    this._group.InsertSpecialItem(this._group.Items.Count, CollectionView.NewItemPlaceholder, true);
                }
            }
        }

        private IList PrepareLocalArray(IList list)
        {
            ArrayList list2;
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (this.ActiveFilter == null)
            {
                list2 = new ArrayList(list);
                if (this.IsAddingNew)
                {
                    list2.RemoveAt(this._newItemIndex);
                }
            }
            else
            {
                list2 = new ArrayList(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    if (this.ActiveFilter(list[i]) && (!this.IsAddingNew || (i != this._newItemIndex)))
                    {
                        list2.Add(list[i]);
                    }
                }
            }
            if (this.ActiveComparer != null)
            {
                list2.Sort(this.ActiveComparer);
            }
            return list2;
        }

        private void PrepareSortAndFilter(IList list)
        {
            if (this._customSort != null)
            {
                this.ActiveComparer = this._customSort;
            }
            else if ((this._sort != null) && (this._sort.Count > 0))
            {
                if (this.SourceCollection.GetType().Name == "XmlDataCollection")
                {
                    this.PrepareXmlComparer();
                }
                else
                {
                    this.ActiveComparer = new SortFieldComparer(this._sort, this.Culture);
                }
            }
            else
            {
                this.ActiveComparer = null;
            }
            this.ActiveFilter = this.Filter;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void PrepareXmlComparer()
        {
            XmlDataCollection sourceCollection = this.SourceCollection as XmlDataCollection;
            Invariant.Assert(this._sort != null);
            this.ActiveComparer = new XmlNodeComparer(this._sort, sourceCollection.XmlNamespaceManager, this.Culture);
        }

        protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            this.ValidateCollectionChangedEventArgs(args);
            int adjustedOldIndex = -1;
            int adjustedNewIndex = -1;
            if (base.UpdatedOutsideDispatcher)
            {
                if (this._applyChangeToShadow && (args.Action != NotifyCollectionChangedAction.Reset))
                {
                    if (((args.Action != NotifyCollectionChangedAction.Remove) && (args.NewStartingIndex < 0)) || ((args.Action != NotifyCollectionChangedAction.Add) && (args.OldStartingIndex < 0)))
                    {
                        return;
                    }
                    this.AdjustShadowCopy(args);
                }
                this._applyChangeToShadow = true;
            }
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                if (this.IsEditingItem)
                {
                    this.ImplicitlyCancelEdit();
                }
                if (this.IsAddingNew)
                {
                    this._newItemIndex = this.SourceList.IndexOf(this._newItem);
                    if (this._newItemIndex < 0)
                    {
                        this.EndAddNew(true);
                    }
                }
                base.RefreshOrDefer();
            }
            else if ((args.Action == NotifyCollectionChangedAction.Add) && (this._newItemIndex == -2))
            {
                this.BeginAddNew(args.NewItems[0], args.NewStartingIndex);
            }
            else
            {
                if (args.Action != NotifyCollectionChangedAction.Remove)
                {
                    adjustedNewIndex = this.AdjustBefore(NotifyCollectionChangedAction.Add, args.NewItems[0], args.NewStartingIndex);
                }
                if (args.Action != NotifyCollectionChangedAction.Add)
                {
                    adjustedOldIndex = this.AdjustBefore(NotifyCollectionChangedAction.Remove, args.OldItems[0], args.OldStartingIndex);
                    if ((this.UsesLocalArray && (adjustedOldIndex >= 0)) && (adjustedOldIndex < adjustedNewIndex))
                    {
                        adjustedNewIndex--;
                    }
                }
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (args.NewStartingIndex <= this._newItemIndex)
                        {
                            this._newItemIndex++;
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        {
                            if (args.OldStartingIndex < this._newItemIndex)
                            {
                                this._newItemIndex--;
                            }
                            object obj2 = args.OldItems[0];
                            if (obj2 == this.CurrentEditItem)
                            {
                                this.ImplicitlyCancelEdit();
                            }
                            else if (obj2 == this.CurrentAddItem)
                            {
                                this.EndAddNew(true);
                            }
                            break;
                        }
                    case NotifyCollectionChangedAction.Move:
                        if ((args.OldStartingIndex >= this._newItemIndex) || (this._newItemIndex >= args.NewStartingIndex))
                        {
                            if ((args.NewStartingIndex <= this._newItemIndex) && (this._newItemIndex < args.OldStartingIndex))
                            {
                                this._newItemIndex++;
                            }
                            break;
                        }
                        this._newItemIndex--;
                        break;
                }
                this.ProcessCollectionChangedWithAdjustedIndex(args, adjustedOldIndex, adjustedNewIndex);
            }
        }

        private void ProcessCollectionChangedWithAdjustedIndex(NotifyCollectionChangedEventArgs args, int adjustedOldIndex, int adjustedNewIndex)
        {
            NotifyCollectionChangedAction replace = args.Action;
            if ((adjustedOldIndex == adjustedNewIndex) && (adjustedOldIndex >= 0))
            {
                replace = NotifyCollectionChangedAction.Replace;
            }
            else if (adjustedOldIndex == -1)
            {
                if (adjustedNewIndex < 0)
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                    {
                        return;
                    }
                    replace = NotifyCollectionChangedAction.Remove;
                }
            }
            else if (adjustedOldIndex < -1)
            {
                if (adjustedNewIndex < 0)
                {
                    return;
                }
                replace = NotifyCollectionChangedAction.Add;
            }
            else if (adjustedNewIndex < 0)
            {
                replace = NotifyCollectionChangedAction.Remove;
            }
            else
            {
                replace = NotifyCollectionChangedAction.Move;
            }
            int num = this.IsGrouping ? 0 : ((this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? (this.IsAddingNew ? 2 : 1) : 0);
            int currentPosition = this.CurrentPosition;
            int num3 = this.CurrentPosition;
            object currentItem = this.CurrentItem;
            bool isCurrentAfterLast = this.IsCurrentAfterLast;
            bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
            NotifyCollectionChangedEventArgs args2 = null;
            switch (replace)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((this.UsesLocalArray && (CollectionView.NewItemPlaceholder != args.NewItems[0])) && (!this.IsAddingNew || !object.Equals(this._newItem, args.NewItems[0])))
                    {
                        this.InternalList.Insert(adjustedNewIndex - num, args.NewItems[0]);
                    }
                    if (!this.IsGrouping)
                    {
                        this.AdjustCurrencyForAdd(adjustedNewIndex);
                        args = new NotifyCollectionChangedEventArgs(replace, args.NewItems[0], adjustedNewIndex);
                    }
                    else
                    {
                        this.AddItemToGroups(args.NewItems[0]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (this.UsesLocalArray)
                    {
                        int index = adjustedOldIndex - num;
                        if ((index < this.InternalList.Count) && object.Equals(this.InternalList[index], args.OldItems[0]))
                        {
                            this.InternalList.RemoveAt(index);
                        }
                    }
                    if (!this.IsGrouping)
                    {
                        this.AdjustCurrencyForRemove(adjustedOldIndex);
                        args = new NotifyCollectionChangedEventArgs(replace, args.OldItems[0], adjustedOldIndex);
                    }
                    else
                    {
                        this.RemoveItemFromGroups(args.OldItems[0]);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (this.UsesLocalArray)
                    {
                        this.InternalList[adjustedOldIndex - num] = args.NewItems[0];
                    }
                    if (!this.IsGrouping)
                    {
                        this.AdjustCurrencyForReplace(adjustedOldIndex);
                        args = new NotifyCollectionChangedEventArgs(replace, args.NewItems[0], args.OldItems[0], adjustedOldIndex);
                    }
                    else
                    {
                        this.RemoveItemFromGroups(args.OldItems[0]);
                        this.AddItemToGroups(args.NewItems[0]);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    {
                        bool flag3 = args.OldItems[0] == args.NewItems[0];
                        if (this.UsesLocalArray)
                        {
                            int num5 = adjustedOldIndex - num;
                            int num6 = adjustedNewIndex - num;
                            if ((num5 < this.InternalList.Count) && object.Equals(this.InternalList[num5], args.OldItems[0]))
                            {
                                this.InternalList.RemoveAt(num5);
                            }
                            if (CollectionView.NewItemPlaceholder != args.NewItems[0])
                            {
                                this.InternalList.Insert(num6, args.NewItems[0]);
                            }
                        }
                        if (!this.IsGrouping)
                        {
                            this.AdjustCurrencyForMove(adjustedOldIndex, adjustedNewIndex);
                            if (flag3)
                            {
                                args = new NotifyCollectionChangedEventArgs(replace, args.OldItems[0], adjustedNewIndex, adjustedOldIndex);
                            }
                            else
                            {
                                args2 = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, args.NewItems, adjustedNewIndex);
                                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, args.OldItems, adjustedOldIndex);
                            }
                        }
                        else if (!flag3)
                        {
                            this.RemoveItemFromGroups(args.OldItems[0]);
                            this.AddItemToGroups(args.NewItems[0]);
                        }
                        break;
                    }
                default:
                    Invariant.Assert(false, SR.Get("UnexpectedCollectionChangeAction", new object[] { replace }));
                    break;
            }
            bool flag4 = this.IsCurrentAfterLast != isCurrentAfterLast;
            bool flag5 = this.IsCurrentBeforeFirst != isCurrentBeforeFirst;
            bool flag6 = this.CurrentPosition != num3;
            bool flag7 = this.CurrentItem != currentItem;
            isCurrentAfterLast = this.IsCurrentAfterLast;
            isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
            num3 = this.CurrentPosition;
            currentItem = this.CurrentItem;
            if (!this.IsGrouping)
            {
                this.OnCollectionChanged(args);
                if (args2 != null)
                {
                    this.OnCollectionChanged(args2);
                }
                if (this.IsCurrentAfterLast != isCurrentAfterLast)
                {
                    flag4 = false;
                    isCurrentAfterLast = this.IsCurrentAfterLast;
                }
                if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
                {
                    flag5 = false;
                    isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
                }
                if (this.CurrentPosition != num3)
                {
                    flag6 = false;
                    num3 = this.CurrentPosition;
                }
                if (this.CurrentItem != currentItem)
                {
                    flag7 = false;
                    currentItem = this.CurrentItem;
                }
            }
            if (this._currentElementWasRemoved)
            {
                this.MoveCurrencyOffDeletedElement(currentPosition);
                flag4 = flag4 || (this.IsCurrentAfterLast != isCurrentAfterLast);
                flag5 = flag5 || (this.IsCurrentBeforeFirst != isCurrentBeforeFirst);
                flag6 = flag6 || (this.CurrentPosition != num3);
                flag7 = flag7 || (this.CurrentItem != currentItem);
            }
            if (flag4)
            {
                this.OnPropertyChanged("IsCurrentAfterLast");
            }
            if (flag5)
            {
                this.OnPropertyChanged("IsCurrentBeforeFirst");
            }
            if (flag6)
            {
                this.OnPropertyChanged("CurrentPosition");
            }
            if (flag7)
            {
                this.OnPropertyChanged("CurrentItem");
            }
        }

        protected override void RefreshOverride()
        {
            lock (base.SyncRoot)
            {
                base.ClearChangeLog();
                if (base.UpdatedOutsideDispatcher)
                {
                    this.ShadowCollection = new ArrayList((ICollection)this.SourceCollection);
                }
            }
            object currentItem = this.CurrentItem;
            int num = this.IsEmpty ? -1 : this.CurrentPosition;
            bool isCurrentAfterLast = this.IsCurrentAfterLast;
            bool isCurrentBeforeFirst = this.IsCurrentBeforeFirst;
            base.OnCurrentChanging();
            IList list = base.UpdatedOutsideDispatcher ? this.ShadowCollection : (this.SourceCollection as IList);
            this.PrepareSortAndFilter(list);
            if (!this.UsesLocalArray)
            {
                this._internalList = list;
            }
            else
            {
                this._internalList = this.PrepareLocalArray(list);
            }
            this.PrepareGroups();
            if (isCurrentBeforeFirst || this.IsEmpty)
            {
                base.SetCurrent(null, -1);
            }
            else if (isCurrentAfterLast)
            {
                base.SetCurrent(null, this.InternalCount);
            }
            else
            {
                int index = this.InternalIndexOf(currentItem);
                if (index < 0)
                {
                    object obj3;
                    index = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
                    if ((index < this.InternalCount) && ((obj3 = this.InternalItemAt(index)) != CollectionView.NewItemPlaceholder))
                    {
                        base.SetCurrent(obj3, index);
                    }
                    else
                    {
                        base.SetCurrent(null, -1);
                    }
                }
                else
                {
                    base.SetCurrent(currentItem, index);
                }
            }
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnCurrentChanged();
            if (this.IsCurrentAfterLast != isCurrentAfterLast)
            {
                this.OnPropertyChanged("IsCurrentAfterLast");
            }
            if (this.IsCurrentBeforeFirst != isCurrentBeforeFirst)
            {
                this.OnPropertyChanged("IsCurrentBeforeFirst");
            }
            if (num != this.CurrentPosition)
            {
                this.OnPropertyChanged("CurrentPosition");
            }
            if (currentItem != this.CurrentItem)
            {
                this.OnPropertyChanged("CurrentItem");
            }
        }

        public void Remove(object item)
        {
            int index = this.InternalIndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            if (this.IsEditingItem || this.IsAddingNew)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "RemoveAt" }));
            }
            base.VerifyRefreshNotDeferred();
            int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning) ? 1 : 0;
            object itemAt = this.GetItemAt(index);
            if (itemAt == CollectionView.NewItemPlaceholder)
            {
                throw new InvalidOperationException(SR.Get("RemovingPlaceholder"));
            }
            int num2 = index - num;
            bool flag = !(this.SourceList is INotifyCollectionChanged);
            if (this.UsesLocalArray || this.IsGrouping)
            {
                if (flag)
                {
                    num2 = this.SourceList.IndexOf(itemAt);
                    this.SourceList.RemoveAt(num2);
                }
                else
                {
                    this.SourceList.Remove(itemAt);
                }
            }
            else
            {
                this.SourceList.RemoveAt(num2);
            }
            if (flag)
            {
                this.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, itemAt, num2));
            }
        }

        private void RemoveItemFromGroups(object item)
        {
            if (this.CanGroupNamesChange || this._group.RemoveFromSubgroups(item))
            {
                this._group.RemoveItemFromSubgroupsByExhaustiveSearch(item);
            }
        }

        private void SetSortDescriptions(SortDescriptionCollection descriptions)
        {
            if (this._sort != null)
            {
                this._sort.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
            }
            this._sort = descriptions;
            if (this._sort != null)
            {
                Invariant.Assert(this._sort.Count == 0, "must be empty SortDescription collection");
                this._sort.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SortDescriptionsChanged);
            }
        }

        private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.IsAddingNew || this.IsEditingItem)
            {
                throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "Sorting" }));
            }
            if (this._sort.Count > 0)
            {
                this._customSort = null;
            }
            base.RefreshOrDefer();
        }

        int IComparer.Compare(object o1, object o2)
        {
            return this.Compare(o1, o2);
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
                    break;

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

        // Properties
        protected IComparer ActiveComparer
        {
            get
            {
                return this._activeComparer;
            }
            set
            {
                this._activeComparer = value;
                this._group.ActiveComparer = value;
            }
        }

        protected Predicate<object> ActiveFilter
        {
            get
            {
                return this._activeFilter;
            }
            set
            {
                this._activeFilter = value;
            }
        }

        public bool CanAddNew
        {
            get
            {
                return ((!this.IsEditingItem && !this.SourceList.IsFixedSize) && this.CanConstructItem);
            }
        }

        public bool CanCancelEdit
        {
            get
            {
                return (this._editItem is IEditableObject);
            }
        }

        private bool CanConstructItem
        {
            get
            {
                if (!this._isItemConstructorValid)
                {
                    this.EnsureItemConstructor();
                }
                return (this._itemConstructor != null);
            }
        }

        public override bool CanFilter
        {
            get
            {
                return true;
            }
        }

        public override bool CanGroup
        {
            get
            {
                return true;
            }
        }

        private bool CanGroupNamesChange
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
                return ((!this.IsEditingItem && !this.IsAddingNew) && !this.SourceList.IsFixedSize);
            }
        }

        public override bool CanSort
        {
            get
            {
                return true;
            }
        }

        public override int Count
        {
            get
            {
                base.VerifyRefreshNotDeferred();
                return this.InternalCount;
            }
        }

        public object CurrentAddItem
        {
            get
            {
                return this._newItem;
            }
        }

        public object CurrentEditItem
        {
            get
            {
                return this._editItem;
            }
        }

        public IComparer CustomSort
        {
            get
            {
                return this._customSort;
            }
            set
            {
                if (this.IsAddingNew || this.IsEditingItem)
                {
                    throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "CustomSort" }));
                }
                this._customSort = value;
                this.SetSortDescriptions(null);
                base.RefreshOrDefer();
            }
        }

        public override Predicate<object> Filter
        {
            get
            {
                return base.Filter;
            }
            set
            {
                if (this.IsAddingNew || this.IsEditingItem)
                {
                    throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "Filter" }));
                }
                base.Filter = value;
            }
        }

        [DefaultValue((string)null)]
        public virtual GroupDescriptionSelectorCallback GroupBySelector
        {
            get
            {
                return this._group.GroupBySelector;
            }
            set
            {
                if (!this.CanGroup)
                {
                    throw new NotSupportedException();
                }
                if (this.IsAddingNew || this.IsEditingItem)
                {
                    throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringAddOrEdit", new object[] { "Grouping" }));
                }
                this._group.GroupBySelector = value;
                base.RefreshOrDefer();
            }
        }

        public override ObservableCollection<GroupDescription> GroupDescriptions
        {
            get
            {
                return this._group.GroupDescriptions;
            }
        }

        public override ReadOnlyObservableCollection<object> Groups
        {
            get
            {
                if (!this.IsGrouping)
                {
                    return null;
                }
                return this._group.Items;
            }
        }

        internal bool HasSortDescriptions
        {
            get
            {
                return ((this._sort != null) && (this._sort.Count > 0));
            }
        }

        protected int InternalCount
        {
            get
            {
                if (this.IsGrouping)
                {
                    return this._group.ItemCount;
                }
                int num = (this.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None) ? 0 : 1;
                if (this.UsesLocalArray && this.IsAddingNew)
                {
                    num++;
                }
                return (num + this.InternalList.Count);
            }
        }

        protected IList InternalList
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

        private bool IsCurrentInView
        {
            get
            {
                return ((0 <= this.CurrentPosition) && (this.CurrentPosition < this.InternalCount));
            }
        }

        public bool IsDataInGroupOrder
        {
            get
            {
                return this._group.IsDataInGroupOrder;
            }
            set
            {
                this._group.IsDataInGroupOrder = value;
            }
        }

        public bool IsEditingItem
        {
            get
            {
                return (this._editItem != null);
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return (this.InternalCount == 0);
            }
        }

        protected bool IsGrouping
        {
            get
            {
                return this._isGrouping;
            }
        }

        public ReadOnlyCollection<ItemPropertyInfo> ItemProperties
        {
            get
            {
                return base.GetItemProperties();
            }
        }

        public NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get
            {
                return this._newItemPlaceholderPosition;
            }
            set
            {
                base.VerifyRefreshNotDeferred();
                if ((value != this._newItemPlaceholderPosition) && this.IsAddingNew)
                {
                    throw new InvalidOperationException(SR.Get("MemberNotAllowedDuringTransaction", new object[] { "NewItemPlaceholderPosition", "AddNew" }));
                }
                NotifyCollectionChangedEventArgs args = null;
                int index = -1;
                int internalCount = -1;
                switch (value)
                {
                    case NewItemPlaceholderPosition.None:
                        switch (this._newItemPlaceholderPosition)
                        {
                            case NewItemPlaceholderPosition.AtBeginning:
                                index = 0;
                                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, CollectionView.NewItemPlaceholder, index);
                                break;

                            case NewItemPlaceholderPosition.AtEnd:
                                index = this.InternalCount - 1;
                                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, CollectionView.NewItemPlaceholder, index);
                                break;
                        }
                        break;

                    case NewItemPlaceholderPosition.AtBeginning:
                        switch (this._newItemPlaceholderPosition)
                        {
                            case NewItemPlaceholderPosition.None:
                                internalCount = 0;
                                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, CollectionView.NewItemPlaceholder, internalCount);
                                break;

                            case NewItemPlaceholderPosition.AtEnd:
                                index = this.InternalCount - 1;
                                internalCount = 0;
                                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, CollectionView.NewItemPlaceholder, internalCount, index);
                                break;
                        }
                        break;

                    case NewItemPlaceholderPosition.AtEnd:
                        switch (this._newItemPlaceholderPosition)
                        {
                            case NewItemPlaceholderPosition.None:
                                internalCount = this.InternalCount;
                                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, CollectionView.NewItemPlaceholder, internalCount);
                                break;

                            case NewItemPlaceholderPosition.AtBeginning:
                                index = 0;
                                internalCount = this.InternalCount - 1;
                                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, CollectionView.NewItemPlaceholder, internalCount, index);
                                break;
                        }
                        break;
                }
                if (args != null)
                {
                    this._newItemPlaceholderPosition = value;
                    if (!this.IsGrouping)
                    {
                        this.ProcessCollectionChangedWithAdjustedIndex(args, index, internalCount);
                    }
                    else
                    {
                        if (index >= 0)
                        {
                            int num3 = (index == 0) ? 0 : (this._group.Items.Count - 1);
                            this._group.RemoveSpecialItem(num3, CollectionView.NewItemPlaceholder, false);
                        }
                        if (internalCount >= 0)
                        {
                            int num4 = (internalCount == 0) ? 0 : this._group.Items.Count;
                            this._group.InsertSpecialItem(num4, CollectionView.NewItemPlaceholder, false);
                        }
                    }
                    this.OnPropertyChanged("NewItemPlaceholderPosition");
                }
            }
        }

        internal ArrayList ShadowCollection
        {
            get
            {
                return this._shadowCollection;
            }
            set
            {
                this._shadowCollection = value;
            }
        }

        public override SortDescriptionCollection SortDescriptions
        {
            get
            {
                if (this._sort == null)
                {
                    this.SetSortDescriptions(new SortDescriptionCollection());
                }
                return this._sort;
            }
        }

        private IList SourceList
        {
            get
            {
                return (this.SourceCollection as IList);
            }
        }

        protected bool UsesLocalArray
        {
            get
            {
                if (this.ActiveComparer == null)
                {
                    return (this.ActiveFilter != null);
                }
                return true;
            }
        }

        // Nested Types
        private class ListOrdinalComparer : IComparer
        {
            // Fields
            private IList _ilFull;
            private int _index;
            private object _item;

            // Methods
            internal ListOrdinalComparer(IList ilFull, object item, int index)
            {
                this._ilFull = ilFull;
                this._item = item;
                this._index = index;
            }

            public int Compare(object o1, object o2)
            {
                int num = object.Equals(o1, this._item) ? this._index : this._ilFull.IndexOf(o1);
                int num2 = object.Equals(o2, this._item) ? this._index : this._ilFull.IndexOf(o2);
                return (num - num2);
            }
        }
    }
}