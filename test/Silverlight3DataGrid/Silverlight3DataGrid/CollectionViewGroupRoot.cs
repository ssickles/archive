using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using System.Windows.Data;
using System.Globalization;

namespace Silverlight3DataGrid
{
    internal class CollectionViewGroupRoot : CollectionViewGroupInternal, INotifyCollectionChanged
    {
        // Fields
        private ObservableCollection<GroupDescription> _groupBy;
        private bool _isDataInGroupOrder;
        private ICollectionView _view;
        private const string RootName = "Root";
        private static GroupDescription topLevelGroupDescription;
        private static readonly object UseAsItemDirectly = new object();

        // Events
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        internal event EventHandler GroupDescriptionChanged;

        // Methods
        internal CollectionViewGroupRoot(ICollectionView view, bool isDataInGroupOrder)
            : base("Root", null)
        {
            this._groupBy = new ObservableCollection<GroupDescription>();
            this._view = view;
            this._isDataInGroupOrder = isDataInGroupOrder;
        }

        private void AddToSubgroup(object item, CollectionViewGroupInternal group, int level, object name, bool loading)
        {
            CollectionViewGroupInternal internal2;
            int num = this._isDataInGroupOrder ? group.LastIndex : 0;
            int count = group.Items.Count;
            while (num < count)
            {
                internal2 = group.Items[num] as CollectionViewGroupInternal;
                if ((internal2 != null) && group.GroupBy.NamesMatch(internal2.Name, name))
                {
                    group.LastIndex = num;
                    this.AddToSubgroups(item, internal2, level + 1, loading);
                    return;
                }
                num++;
            }
            internal2 = new CollectionViewGroupInternal(name, group);
            this.InitializeGroup(internal2, level + 1, item);
            if (loading)
            {
                group.Add(internal2);
                group.LastIndex = num;
            }
            else
            {
                group.Insert(internal2, item, this.ActiveComparer);
            }
            this.AddToSubgroups(item, internal2, level + 1, loading);
        }

        internal void AddToSubgroups(object item, bool loading)
        {
            this.AddToSubgroups(item, this, 0, loading);
        }

        private void AddToSubgroups(object item, CollectionViewGroupInternal group, int level, bool loading)
        {
            object name = this.GetGroupName(item, group.GroupBy, level);
            if (name == UseAsItemDirectly)
            {
                if (loading)
                {
                    group.Add(item);
                }
                else
                {
                    int index = group.Insert(item, item, this.ActiveComparer);
                    int num2 = group.LeafIndexFromItem(item, index);
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(0, item, num2));
                }
            }
            else
            {
                ICollection is2 = name as ICollection;
                if (is2 == null)
                {
                    this.AddToSubgroup(item, group, level, name, loading);
                }
                else
                {
                    foreach (object obj3 in is2)
                    {
                        this.AddToSubgroup(item, group, level, obj3, loading);
                    }
                }
            }
        }

        protected override int FindIndex(object item, object seed, IComparer comparer, int low, int high)
        {
            IEditableCollectionView view = this._view as IEditableCollectionView;
            if ((view != null) && view.IsAddingNew)
            {
                high--;
            }
            return base.FindIndex(item, seed, comparer, low, high);
        }

        private GroupDescription GetGroupDescription(CollectionViewGroup group, int level)
        {
            GroupDescription description = null;
            if (group == this)
            {
                group = null;
            }
            if ((description == null) && (this.GroupBySelector != null))
            {
                description = this.GroupBySelector(group, level);
            }
            if ((description == null) && (level < this.GroupDescriptions.Count))
            {
                description = this.GroupDescriptions[level];
            }
            return description;
        }

        private object GetGroupName(object item, GroupDescription groupDescription, int level)
        {
            if (groupDescription != null)
            {
                return groupDescription.GroupNameFromItem(item, level, this.Culture);
            }
            return UseAsItemDirectly;
        }

        internal void Initialize()
        {
            if (topLevelGroupDescription == null)
            {
                topLevelGroupDescription = new TopLevelGroupDescription();
            }
            this.InitializeGroup(this, 0, null);
        }

        private void InitializeGroup(CollectionViewGroupInternal group, int level, object seedItem)
        {
            GroupDescription groupDescription = this.GetGroupDescription(group, level);
            group.GroupBy = groupDescription;
            ObservableCollection<object> observables = (groupDescription != null) ? groupDescription.get_GroupNames() : null;
            if (observables != null)
            {
                int num = 0;
                int count = observables.Count;
                while (num < count)
                {
                    CollectionViewGroupInternal internal2 = new CollectionViewGroupInternal(observables[num], group);
                    this.InitializeGroup(internal2, level + 1, seedItem);
                    group.Add(internal2);
                    num++;
                }
            }
            group.LastIndex = 0;
        }

        internal void InsertSpecialItem(int index, object item, bool loading)
        {
            base.ChangeCounts(item, 1);
            base.ProtectedItems.Insert(index, item);
            if (!loading)
            {
                int num = base.LeafIndexFromItem(item, index);
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(0, item, num));
            }
        }

        public void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged.Invoke(this, args);
            }
        }

        protected override void OnGroupByChanged()
        {
            if (this.GroupDescriptionChanged != null)
            {
                this.GroupDescriptionChanged(this, EventArgs.Empty);
            }
        }

        private bool RemoveFromGroupDirectly(CollectionViewGroupInternal group, object item)
        {
            int num = group.Remove(item, true);
            if (num >= 0)
            {
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(1, item, num));
                return false;
            }
            return true;
        }

        private bool RemoveFromSubgroup(object item, CollectionViewGroupInternal group, int level, object name)
        {
            bool flag = false;
            int num = 0;
            int count = group.Items.Count;
            while (num < count)
            {
                CollectionViewGroupInternal internal2 = group.Items[num] as CollectionViewGroupInternal;
                if ((internal2 != null) && group.GroupBy.NamesMatch(internal2.Name, name))
                {
                    if (this.RemoveFromSubgroups(item, internal2, level + 1))
                    {
                        flag = true;
                    }
                    return flag;
                }
                num++;
            }
            return true;
        }

        internal bool RemoveFromSubgroups(object item)
        {
            return this.RemoveFromSubgroups(item, this, 0);
        }

        private bool RemoveFromSubgroups(object item, CollectionViewGroupInternal group, int level)
        {
            bool flag = false;
            object name = this.GetGroupName(item, group.GroupBy, level);
            if (name == UseAsItemDirectly)
            {
                return this.RemoveFromGroupDirectly(group, item);
            }
            ICollection is2 = name as ICollection;
            if (is2 == null)
            {
                if (this.RemoveFromSubgroup(item, group, level, name))
                {
                    flag = true;
                }
                return flag;
            }
            foreach (object obj3 in is2)
            {
                if (this.RemoveFromSubgroup(item, group, level, obj3))
                {
                    flag = true;
                }
            }
            return flag;
        }

        internal void RemoveItemFromSubgroupsByExhaustiveSearch(object item)
        {
            this.RemoveItemFromSubgroupsByExhaustiveSearch(this, item);
        }

        private void RemoveItemFromSubgroupsByExhaustiveSearch(CollectionViewGroupInternal group, object item)
        {
            if (this.RemoveFromGroupDirectly(group, item))
            {
                for (int i = group.Items.Count - 1; i >= 0; i--)
                {
                    CollectionViewGroupInternal internal2 = group.Items[i] as CollectionViewGroupInternal;
                    if (internal2 != null)
                    {
                        this.RemoveItemFromSubgroupsByExhaustiveSearch(internal2, item);
                    }
                }
            }
        }

        internal void RemoveSpecialItem(int index, object item, bool loading)
        {
            int num = -1;
            if (!loading)
            {
                num = base.LeafIndexFromItem(item, index);
            }
            base.ChangeCounts(item, -1);
            base.ProtectedItems.RemoveAt(index);
            if (!loading)
            {
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(1, item, num));
            }
        }

        // Properties
        internal IComparer ActiveComparer
        {
            get;
            set;
        }

        internal CultureInfo Culture
        {
            get
            {
                return this._view.get_Culture();
            }
        }

        public virtual GroupDescriptionSelectorCallback GroupBySelector
        {
            get;
            set;
        }

        public virtual ObservableCollection<GroupDescription> GroupDescriptions
        {
            get
            {
                return this._groupBy;
            }
        }

        internal bool IsDataInGroupOrder
        {
            get
            {
                return this._isDataInGroupOrder;
            }
            set
            {
                this._isDataInGroupOrder = value;
            }
        }

        // Nested Types
        private class TopLevelGroupDescription : GroupDescription
        {
            // Methods
            public override object GroupNameFromItem(object item, int level, CultureInfo culture)
            {
                return null;
            }
        }
    }
}
