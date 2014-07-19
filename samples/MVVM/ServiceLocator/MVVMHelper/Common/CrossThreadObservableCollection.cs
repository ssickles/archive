using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MVVMHelper.Commands.Common
{
    public class CrossThreadObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private SynchronizationContext _synchronizationContext;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public CrossThreadObservableCollection()
            : this(SynchronizationContext.Current)
        { }

        public CrossThreadObservableCollection(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
        }

        protected override void ClearItems()
        {
            if (_synchronizationContext != null)
            {
                _synchronizationContext.Send(delegate { ClearItemsImpl(); }, null);
            }
            else
            {
                ClearItemsImpl();
            }
        }

        private void ClearItemsImpl()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void InsertItem(int index, T item)
        {
            if (_synchronizationContext != null)
            {
                _synchronizationContext.Send(delegate { InsertItemImpl(index, item); }, null);
            }
            else
            {
                InsertItemImpl(index, item);
            }
        }

        private void InsertItemImpl(int index, T item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected override void RemoveItem(int index)
        {
            if (_synchronizationContext != null)
            {
                _synchronizationContext.Send(delegate { RemoveItemImpl(index); }, null);
            }
            else
            {
                RemoveItemImpl(index);
            }
        }

        private void RemoveItemImpl(int index)
        {
            T item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        protected override void SetItem(int index, T item)
        {
            if (_synchronizationContext != null)
            {
                _synchronizationContext.Send(delegate { SetItemImpl(index, item); }, null);
            }
            else
            {
                SetItemImpl(index, item);
            }
        }

        private void SetItemImpl(int index, T item)
        {
            T old = this[index];
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, old, index));
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            var collectionChanged = CollectionChanged;
            if (collectionChanged != null)
                collectionChanged(this, e);
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var prop = PropertyChanged;
            if (prop != null)
                prop(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
