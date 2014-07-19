using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CollectionTest
{
    public class ObservableSubCollection<T> : ObservableCollection<T>, IDisposable
    {
        private ObservableCollection<T> _Original;
        private Func<T, bool> _Filter;
        private bool _IgnoreChanges;

        public ObservableSubCollection(ObservableCollection<T> original, Func<T, bool> filter)
            : base(original.Where(filter))
        {
            _Original = original;
            _Filter = filter;
            _Original.CollectionChanged += _Original_CollectionChanged;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_IgnoreChanges)
            {
                _IgnoreChanges = true;
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        _Original.AddRange(e.NewItems);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotSupportedException("Cannot move anything in subcollection.");
                    case NotifyCollectionChangedAction.Remove:
                        _Original.RemoveRange(e.OldItems);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        int replaceIndex = _Original.IndexOf((T)e.OldItems[0]);
                        if (replaceIndex >= 0)
                            _Original[replaceIndex] = (T)e.NewItems[0];
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (T item in _Original.Where(_Filter).ToList())
                        {
                            _Original.Remove(item);
                        }
                        break;
                }
                _IgnoreChanges = false;
            }
            base.OnCollectionChanged(e);
        }

        private void _Original_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_IgnoreChanges)
            {
                _IgnoreChanges = true;
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        this.AddRange(e.NewItems.Where(_Filter));
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotSupportedException("Cannot move anything in collection connected to subcollection.");
                    case NotifyCollectionChangedAction.Remove:
                        this.RemoveRange(e.OldItems);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        int replaceIndex = this.IndexOf((T)e.OldItems[0]);
                        if (replaceIndex >= 0)
                            this[replaceIndex] = (T)e.NewItems[0];
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        this.Clear();
                        break;
                }
                _IgnoreChanges = false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_Original != null)
                _Original.CollectionChanged -= _Original_CollectionChanged;
        }

        #endregion
    }
}
