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
using System.Windows.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;

namespace Silverlight3DataGrid
{
    internal class CollectionViewGroupInternal : CollectionViewGroup
    {
        // Fields
        private GroupDescription _groupBy;
        private CollectionViewGroupInternal _parentGroup;
        private int _version;

        // Methods
        internal CollectionViewGroupInternal(object name, CollectionViewGroupInternal parent)
            : base(name)
        {
            this._parentGroup = parent;
        }

        internal void Add(object item)
        {
            this.ChangeCounts(item, 1);
            base.ProtectedItems.Add(item);
        }

        protected void ChangeCounts(object item, int delta)
        {
            bool flag = !(item is CollectionViewGroup);
            for (CollectionViewGroupInternal internal2 = this; internal2 != null; internal2 = internal2._parentGroup)
            {
                internal2.FullCount += delta;
                if (flag)
                {
                    internal2.ProtectedItemCount += delta;
                    if (internal2.ProtectedItemCount == 0)
                    {
                        RemoveEmptyGroup(internal2);
                    }
                }
            }
            this._version++;
        }

        internal void Clear()
        {
            base.ProtectedItems.Clear();
            this.FullCount = 1;
            base.ProtectedItemCount = 0;
        }

        protected virtual int FindIndex(object item, object seed, IComparer comparer, int low, int high)
        {
            if (comparer != null)
            {
                ListComparer comparer2 = comparer as ListComparer;
                if (comparer2 != null)
                {
                    comparer2.Reset();
                }
                CollectionViewGroupComparer comparer3 = comparer as CollectionViewGroupComparer;
                if (comparer3 != null)
                {
                    comparer3.Reset();
                }
                int num = low;
                while (num < high)
                {
                    CollectionViewGroupInternal internal2 = base.ProtectedItems[num] as CollectionViewGroupInternal;
                    object y = (internal2 != null) ? internal2.SeedItem : base.ProtectedItems[num];
                    if ((y != DependencyProperty.UnsetValue) && (comparer.Compare(seed, y) < 0))
                    {
                        return num;
                    }
                    num++;
                }
                return num;
            }
            return high;
        }

        internal IEnumerator GetLeafEnumerator()
        {
            return new LeafEnumerator(this);
        }

        internal int Insert(object item, object seed, IComparer comparer)
        {
            int low = (this.GroupBy == null) ? 0 : this.GroupBy.GroupNames.Count;
            int index = this.FindIndex(item, seed, comparer, low, base.ProtectedItems.Count);
            this.ChangeCounts(item, 1);
            base.ProtectedItems.Insert(index, item);
            return index;
        }

        internal object LeafAt(int index)
        {
            int num = 0;
            int count = base.Items.Count;
            while (num < count)
            {
                CollectionViewGroupInternal internal2 = base.Items[num] as CollectionViewGroupInternal;
                if (internal2 != null)
                {
                    if (index < internal2.ItemCount)
                    {
                        return internal2.LeafAt(index);
                    }
                    index -= internal2.ItemCount;
                }
                else
                {
                    if (index == 0)
                    {
                        return base.Items[num];
                    }
                    index--;
                }
                num++;
            }
            return null;
        }

        internal int LeafIndexFromItem(object item, int index)
        {
            int num = 0;
            CollectionViewGroupInternal parent = this;
            while (parent != null)
            {
                int num2 = 0;
                int count = parent.Items.Count;
                while (num2 < count)
                {
                    if (((index < 0) && object.Equals(item, parent.Items[num2])) || (index == num2))
                    {
                        break;
                    }
                    CollectionViewGroupInternal internal3 = parent.Items[num2] as CollectionViewGroupInternal;
                    num += (internal3 == null) ? 1 : internal3.ItemCount;
                    num2++;
                }
                item = parent;
                parent = parent.Parent;
                index = -1;
            }
            return num;
        }

        internal int LeafIndexOf(object item)
        {
            int num = 0;
            int num2 = 0;
            int count = base.Items.Count;
            while (num2 < count)
            {
                CollectionViewGroupInternal internal2 = base.Items[num2] as CollectionViewGroupInternal;
                if (internal2 != null)
                {
                    int num4 = internal2.LeafIndexOf(item);
                    if (num4 >= 0)
                    {
                        return (num + num4);
                    }
                    num += internal2.ItemCount;
                }
                else
                {
                    if (object.Equals(item, base.Items[num2]))
                    {
                        return num;
                    }
                    num++;
                }
                num2++;
            }
            return -1;
        }

        protected virtual void OnGroupByChanged()
        {
            if (this.Parent != null)
            {
                this.Parent.OnGroupByChanged();
            }
        }

        private void OnGroupByChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnGroupByChanged();
        }

        internal int Remove(object item, bool returnLeafIndex)
        {
            int num = -1;
            int index = base.ProtectedItems.IndexOf(item);
            if (index >= 0)
            {
                if (returnLeafIndex)
                {
                    num = this.LeafIndexFromItem(null, index);
                }
                this.ChangeCounts(item, -1);
                base.ProtectedItems.RemoveAt(index);
            }
            return num;
        }

        private static void RemoveEmptyGroup(CollectionViewGroupInternal group)
        {
            CollectionViewGroupInternal parent = group.Parent;
            if (parent != null)
            {
                GroupDescription groupBy = parent.GroupBy;
                if (parent.ProtectedItems.IndexOf(group) >= groupBy.GroupNames.Count)
                {
                    parent.Remove(group, false);
                }
            }
        }

        // Properties
        [DefaultValue(1)]
        internal int FullCount
        {
            get;
            set;
        }

        internal GroupDescription GroupBy
        {
            get
            {
                return this._groupBy;
            }
            set
            {
                bool isBottomLevel = this.IsBottomLevel;
                if (this._groupBy != null)
                {
                    this._groupBy.PropertyChanged -= new PropertyChangedEventHandler(this.OnGroupByChanged);
                }
                this._groupBy = value;
                if (this._groupBy != null)
                {
                    this._groupBy.PropertyChanged += new PropertyChangedEventHandler(this.OnGroupByChanged);
                }
                if (isBottomLevel != this.IsBottomLevel)
                {
                    this.OnPropertyChanged(new PropertyChangedEventArgs("IsBottomLevel"));
                }
            }
        }

        public override bool IsBottomLevel
        {
            get
            {
                return (this._groupBy == null);
            }
        }

        internal int LastIndex
        {
            get;
            set;
        }

        private CollectionViewGroupInternal Parent
        {
            get
            {
                return this._parentGroup;
            }
        }

        internal object SeedItem
        {
            get
            {
                if ((base.ItemCount > 0) && ((this.GroupBy == null) || (this.GroupBy.GroupNames.Count == 0)))
                {
                    int num = 0;
                    int count = base.Items.Count;
                    while (num < count)
                    {
                        CollectionViewGroupInternal internal2 = base.Items[num] as CollectionViewGroupInternal;
                        if (internal2 == null)
                        {
                            return base.Items[num];
                        }
                        if (internal2.ItemCount > 0)
                        {
                            return internal2.SeedItem;
                        }
                        num++;
                    }
                }
                return DependencyProperty.UnsetValue;
            }
        }

        // Nested Types
        internal class CollectionViewGroupComparer : IComparer
        {
            // Fields
            private CollectionViewGroupRoot _group;
            private int _index;

            // Methods
            internal CollectionViewGroupComparer(CollectionViewGroupRoot group)
            {
                this.ResetGroup(group);
            }

            public int Compare(object x, object y)
            {
                if (object.Equals(x, y))
                {
                    return 0;
                }
                int num = (this._group != null) ? this._group.ItemCount : 0;
                while (this._index < num)
                {
                    object objB = this._group.LeafAt(this._index);
                    if (object.Equals(x, objB))
                    {
                        return -1;
                    }
                    if (object.Equals(y, objB))
                    {
                        return 1;
                    }
                    this._index++;
                }
                return 1;
            }

            internal void Reset()
            {
                this._index = 0;
            }

            internal void ResetGroup(CollectionViewGroupRoot group)
            {
                this._group = group;
                this._index = 0;
            }
        }

        private class LeafEnumerator : IEnumerator
        {
            // Fields
            private object _current;
            private CollectionViewGroupInternal _group;
            private int _index;
            private IEnumerator _subEnum;
            private int _version;

            // Methods
            public LeafEnumerator(CollectionViewGroupInternal group)
            {
                this._group = group;
                this.DoReset();
            }

            private void DoReset()
            {
                this._version = this._group._version;
                this._index = -1;
                this._subEnum = null;
            }

            bool IEnumerator.MoveNext()
            {
                if (this._group._version != this._version)
                {
                    throw new InvalidOperationException();
                }
                while ((this._subEnum == null) || !this._subEnum.MoveNext())
                {
                    this._index++;
                    if (this._index >= this._group.Items.Count)
                    {
                        return false;
                    }
                    CollectionViewGroupInternal internal2 = this._group.Items[this._index] as CollectionViewGroupInternal;
                    if (internal2 == null)
                    {
                        this._current = this._group.Items[this._index];
                        this._subEnum = null;
                        return true;
                    }
                    this._subEnum = internal2.GetLeafEnumerator();
                }
                this._current = this._subEnum.Current;
                return true;
            }

            void IEnumerator.Reset()
            {
                this.DoReset();
            }

            // Properties
            object IEnumerator.Current
            {
                get
                {
                    if ((this._index < 0) || (this._index >= this._group.Items.Count))
                    {
                        throw new InvalidOperationException();
                    }
                    return this._current;
                }
            }
        }

        internal class ListComparer : IComparer
        {
            // Fields
            private int _index;
            private IList _list;

            // Methods
            internal ListComparer(IList list)
            {
                this.ResetList(list);
            }

            public int Compare(object x, object y)
            {
                if (object.Equals(x, y))
                {
                    return 0;
                }
                int num = (this._list != null) ? this._list.Count : 0;
                while (this._index < num)
                {
                    object objB = this._list[this._index];
                    if (object.Equals(x, objB))
                    {
                        return -1;
                    }
                    if (object.Equals(y, objB))
                    {
                        return 1;
                    }
                    this._index++;
                }
                return 1;
            }

            internal void Reset()
            {
                this._index = 0;
            }

            internal void ResetList(IList list)
            {
                this._list = list;
                this._index = 0;
            }
        }
    }
}
