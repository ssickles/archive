using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using IdentityStream.Data;
using System.Collections;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace System.Windows.Data
{
    public delegate List Func<Query, Total, List>(Query query, out Total totalResults);

    public class RemoteCollectionView<T> : ICollectionView, IPagedCollectionView, IEditableCollectionView, INotifyPropertyChanged, IDisposable
    {
        private CultureInfo _culture;
        private int _currentPosition;
        private bool _isEmpty;
        private ObservableCollection<GroupDescription> _groupDescriptions;
        private ReadOnlyObservableCollection<object> _groups;

        private bool _canChangePage;
        private bool _isPageChanging;
        private int _itemCount;
        private int _pageSize;
        private int _pageIndex;
        private int _totalItemCount;

        private ObservableCollection<T> _collection;
        private Func<QueryObject, int, List<T>> _fetchRoutine;
        private QueryExpression _queryExpression;
        private int _deferLevel;
        private bool _needsRefresh;
        private Predicate<object> _filter;
        private IdentityStream.Data.SortDescriptionCollection _sortDescriptions;

        public RemoteCollectionView(Func<QueryObject, int, List<T>> fetchRoutine)
        {
            _fetchRoutine = fetchRoutine;

            _culture = CultureInfo.CurrentUICulture;
            _currentPosition = -1;
            _isEmpty = true;

            _canChangePage = true;
            _isPageChanging = false;
            _itemCount = 0;
            _pageSize = 3;
            _pageIndex = 1;
            _totalItemCount = 0;
            _deferLevel = 0;
            _needsRefresh = true;
            _filter = new Predicate<object>(delegate(object o) { Debug.WriteLine("Execute Filter"); return true; });
            _sortDescriptions = new IdentityStream.Data.SortDescriptionCollection();
            _sortDescriptions.CollectionChanged += new NotifyCollectionChangedEventHandler(SortDescriptions_CollectionChanged);

            _collection = new ObservableCollection<T>(new List<T>());
        }

        public QueryExpression Query
        {
            get { return _queryExpression; }
            set
            {
                if (_queryExpression != value)
                {
                    _queryExpression = value;
                    InternalRefresh();
                }
            }
        }

        #region ICollectionView Members

        public bool CanFilter
        {
            get { Debug.WriteLine("CanFilter"); return false; }
        }
        public bool CanGroup
        {
            get { Debug.WriteLine("CanGroup"); return true; }
        }
        public bool CanSort
        {
            get { Debug.WriteLine("CanSort"); return true; }
        }
        public bool Contains(object item)
        {
            Debug.WriteLine("Contains");
            return _collection.Contains((T)item);
        }
        public CultureInfo Culture
        {
            get
            {
                Debug.WriteLine("Get Culture"); return _culture;
            }
            set
            {
                Debug.WriteLine("Set Culture");
                if (_culture != value)
                {
                    _culture = value;
                    OnPropertyChanged("Culture");
                }
            }
        }
        public event EventHandler CurrentChanged;
        private void OnCurrentChanged()
        {
            EventHandler obj = CurrentChanged;
            if (obj != null)
            {
                CurrentChanged(this, EventArgs.Empty);
            }
        }
        public event CurrentChangingEventHandler CurrentChanging;
        private void OnCurrentChanging()
        {
            CurrentChangingEventHandler obj = CurrentChanging;
            if (obj != null)
            {
                CurrentChanging(this, new CurrentChangingEventArgs(false));
            }
        }
        public object CurrentItem
        {
            get
            {
                if (!IsEmpty && CurrentPosition >= 0 && CurrentPosition < _collection.Count)
                    return _collection[CurrentPosition];
                else
                    return null;
            }
        }
        public int CurrentPosition
        {
            get { return _currentPosition; }
            private set
            {
                if (_currentPosition != value)
                {
                    OnCurrentChanging();
                    _currentPosition = value;
                    OnPropertyChanged("CurrentPosition");
                    OnPropertyChanged("CurrentItem");
                    OnCurrentChanged();
                }
            }
        }
        public IDisposable DeferRefresh()
        {
            Debug.WriteLine("DeferRefresh");
            IEditableCollectionView view = this as IEditableCollectionView;
            if ((view != null) && (view.IsAddingNew || view.IsEditingItem))
            {
                throw new InvalidOperationException("The CollectionView is currently in add or edit mode. You can not defer until these changes have been committed.");
            }
            this._deferLevel++;
            return new DeferHelper(this);
        }
        Predicate<object> ICollectionView.Filter
        {
            get
            {
                Debug.WriteLine("Get Filter"); return _filter;
            }
            set { throw new InvalidOperationException("The filter is not used by this implementation. Use the Query property."); }
        }
        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get
            {
                Debug.WriteLine("Get GroupDescriptions");
                return _groupDescriptions;
            }
        }
        public ReadOnlyObservableCollection<object> Groups
        {
            get { Debug.WriteLine("Get Groups"); return _groups; }
        }
        public bool IsCurrentAfterLast
        {
            get { return false; }
        }
        public bool IsCurrentBeforeFirst
        {
            get { return false; }
        }
        public bool IsEmpty
        {
            get { return _isEmpty; }
            private set
            {
                if (_isEmpty != value)
                {
                    _isEmpty = value;
                    OnPropertyChanged("IsEmpty");
                }
            }
        }
        public bool MoveCurrentTo(object item)
        {
            if (!(item is T))
                return false;
            else
            {
                T castItem = (T)item;
                for (int i = 0; i < _collection.Count; i++)
                {
                    if (_collection[i].Equals(castItem))
                    {
                        CurrentPosition = i;
                        return true;
                    }
                }
                return false;
            }
        }
        public bool MoveCurrentToFirst()
        {
            if (IsEmpty)
                return false;
            else
            {
                CurrentPosition = 0;
                return true;
            }
        }
        public bool MoveCurrentToLast()
        {
            if (IsEmpty)
                return false;
            else
            {
                CurrentPosition = _collection.Count - 1;
                return true;
            }
        }
        public bool MoveCurrentToNext()
        {
            if (CurrentPosition < _collection.Count - 1)
            {
                CurrentPosition++;
                return true;
            }
            else
                return false;
        }
        public bool MoveCurrentToPosition(int position)
        {
            if (position >= 0 && position < _collection.Count - 1)
            {
                CurrentPosition = position;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool MoveCurrentToPrevious()
        {
            if (CurrentPosition > 0)
            {
                CurrentPosition--;
                return true;
            }
            else
                return false;
        }
        public void Refresh()
        {
            _needsRefresh = true;
            InternalRefresh();
        }
        private void InternalRefresh()
        {
            Debug.WriteLine("InternalRefresh");
            if (_deferLevel == 0 && _needsRefresh)
            {
                QueryObject query = new QueryObject(_queryExpression, new List<SortObject>(), _pageSize, (_pageIndex - 1) * _pageSize);
                int totalResults = -1;
                _collection = new ObservableCollection<T>(_fetchRoutine(query, out totalResults));
                ItemCount = _collection.Count;
                TotalItemCount = totalResults;
                OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnPropertyChanged("SourceCollection");
                _needsRefresh = false;
            }
        }
        public System.ComponentModel.SortDescriptionCollection SortDescriptions
        {
            get
            {
                Debug.WriteLine("Get SortDescriptions");
                return _sortDescriptions;
            }
        }
        public IEnumerable SourceCollection
        {
            get { Debug.WriteLine("SourceCollection"); return _collection; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            Debug.WriteLine("GetEnumerator");
            return _collection.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler obj = CollectionChanged;
            if (obj != null)
            {
                CollectionChanged(this, e);
            }
        }

        #endregion

        #region IPagedCollectionView Members

        public bool CanChangePage { get { return _canChangePage; } }
        public bool IsPageChanging
        {
            get { return _isPageChanging; }
            private set
            {
                if (_isPageChanging != value)
                {
                    _isPageChanging = value;
                    OnPropertyChanged("IsPageChanging");
                }
            }
        }
        public int ItemCount
        {
            get { return _itemCount; }
            private set
            {
                if (_itemCount != value)
                {
                    _itemCount = value;
                    OnPropertyChanged("ItemCount");
                }
            }
        }
        public bool MoveToFirstPage()
        {
            return MoveToPage(1);
        }
        public bool MoveToLastPage()
        {
            return MoveToPage(GetPageCount());
        }
        public bool MoveToNextPage()
        {
            if (PageIndex < GetPageCount())
                return MoveToPage(PageIndex + 1);
            else
                return false;
        }
        public bool MoveToPage(int pageIndex)
        {
            if (pageIndex < 1 || pageIndex > GetPageCount() || pageIndex == _pageIndex)
                return false;

            _needsRefresh = true;
            OnPageChanging(pageIndex);
            PageIndex = pageIndex;
            InternalRefresh();
            OnPageChanged();
            return true;
        }
        public bool MoveToPreviousPage()
        {
            if (PageIndex > 1)
                return MoveToPage(PageIndex - 1);
            else
                return false;
        }
        public event EventHandler<EventArgs> PageChanged;
        private void OnPageChanged()
        {
            EventHandler<EventArgs> obj = PageChanged;
            if (obj != null) PageChanged(this, EventArgs.Empty);
        }
        public event EventHandler<PageChangingEventArgs> PageChanging;
        private void OnPageChanging(int newPageIndex)
        {
            EventHandler<PageChangingEventArgs> obj = PageChanging;
            if (obj != null) PageChanging(this, new PageChangingEventArgs(newPageIndex));
        }
        public int PageIndex
        {
            get { return _pageIndex; }
            private set
            {
                if (_pageIndex != value)
                {
                    _pageIndex = value;
                    OnPropertyChanged("PageIndex");
                }
            }
        }
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    MoveToPage(1);
                    OnPropertyChanged("PageSize");
                }
            }
        }
        public int TotalItemCount
        {
            get { return _totalItemCount; }
            private set
            {
                if (_totalItemCount != value)
                {
                    _totalItemCount = value;
                    OnPropertyChanged("TotalItemCount");
                }
            }
        }
        private int GetPageCount()
        {
            int numPages = TotalItemCount / PageSize;
            numPages += (TotalItemCount % PageSize) > 0 ? 1 : 0;
            return numPages;
        }

        #endregion

        #region IEditableCollectionView Members

        public object AddNew()
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }
        public bool CanAddNew
        {
            get { return false; }
        }
        public bool CanCancelEdit
        {
            get { return false; }
        }
        public bool CanRemove
        {
            get { return false; }
        }
        public void CancelEdit()
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }
        public void CancelNew()
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }
        public void CommitEdit()
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }
        public void CommitNew()
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }
        public object CurrentAddItem
        {
            get { return null; }
        }
        public object CurrentEditItem
        {
            get { return null; }
        }
        public void EditItem(object item)
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }
        public bool IsAddingNew
        {
            get { return false; }
        }
        public bool IsEditingItem
        {
            get { return false; }
        }
        public NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get
            {
                return NewItemPlaceholderPosition.None;
            }
            set { }
        }
        public void Remove(object item)
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }
        public void RemoveAt(int index)
        {
            throw new NotImplementedException("This has not been implemented yet.");
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler obj = PropertyChanged;
            if (obj != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _collection = null;
        }

        #endregion

        private void EndDefer()
        {
            _deferLevel--;
            InternalRefresh();
        }
        private void SortDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _pageIndex = 1;
            _needsRefresh = true;
            InternalRefresh();
            OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        private void GroupDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private class DeferHelper : IDisposable
        {
            private RemoteCollectionView<T> _rcv;

            internal DeferHelper(RemoteCollectionView<T> rcv)
            {
                _rcv = rcv;
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (_rcv != null)
                {
                    _rcv.EndDefer();
                    _rcv = null;
                }
            }

            #endregion
        }
    }
}
