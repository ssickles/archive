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
using System.Collections.Specialized;
using System.Collections;
using System.Globalization;
using System.Collections.ObjectModel;
using IdentityStream.Data;
using System.Collections.Generic;

namespace Silverlight3DataGrid
{
    public class RemoteCollectionView<T, Key> : ICollectionView, IPagedCollectionView, IEditableCollectionView, INotifyPropertyChanged
    {
        private PagedCollectionView _view;
        private IRemoteRepository<T, Key> _repository;
        private QueryObject _query;

        public RemoteCollectionView(IRemoteRepository<T, Key> Repository)
        {
            _repository = Repository;
            _view = new PagedCollectionView(Repository.Get(null));
            _view.CollectionChanged += CollectionChanged;
            _view.CurrentChanged += CurrentChanged;
            _view.CurrentChanging += CurrentChanging;
            _view.PageChanged += PageChanged;
            _view.PageChanging += PageChanging;
            _view.PropertyChanged += PropertyChanged;
        }

        public QueryObject Query
        {
            get { return _query; }
            set
            {
                if (_query != value)
                {
                    _query = value;
                    Refresh();
                }
            }
        }

        #region ICollectionView Members

        public bool CanFilter
        {
            get { return _view.CanFilter; }
        }
        public bool CanGroup
        {
            get { return _view.CanGroup; }
        }
        public bool CanSort
        {
            get { return _view.CanSort; }
        }
        public bool Contains(object item)
        {
            //TODO: update this method to use the repository
            return _view.Contains(item);
        }
        public CultureInfo Culture
        {
            get
            {
                return _view.Culture;
            }
            set
            {
                _view.Culture = value;
            }
        }
        public event EventHandler CurrentChanged;
        public event CurrentChangingEventHandler CurrentChanging;
        public object CurrentItem
        {
            get { return _view.CurrentItem; }
        }
        public int CurrentPosition
        {
            get { return _view.CurrentPosition; }
        }
        public IDisposable DeferRefresh()
        {
            return _view.DeferRefresh();
        }
        Predicate<object> ICollectionView.Filter
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { return _view.GroupDescriptions; }
        }
        public ReadOnlyObservableCollection<object> Groups
        {
            get { return _view.Groups; }
        }
        public bool IsCurrentAfterLast
        {
            get { return _view.IsCurrentAfterLast; }
        }
        public bool IsCurrentBeforeFirst
        {
            get { return _view.IsCurrentBeforeFirst; }
        }
        public bool IsEmpty
        {
            get { return _view.IsEmpty; }
        }
        public bool MoveCurrentTo(object item)
        {
            throw new NotImplementedException();
        }
        public bool MoveCurrentToFirst()
        {
            throw new NotImplementedException();
        }
        public bool MoveCurrentToLast()
        {
            throw new NotImplementedException();
        }
        public bool MoveCurrentToNext()
        {
            throw new NotImplementedException();
        }
        public bool MoveCurrentToPosition(int position)
        {
            throw new NotImplementedException();
        }
        public bool MoveCurrentToPrevious()
        {
            throw new NotImplementedException();
        }
        public void Refresh()
        {
            _view.CollectionChanged -= CollectionChanged;
            _view.CurrentChanged -= CurrentChanged;
            _view.CurrentChanging -= CurrentChanging;
            _view.PageChanged -= PageChanged;
            _view.PageChanging -= PageChanging;
            _view.PropertyChanged -= PropertyChanged;

            PagedCollectionView newView = new PagedCollectionView(_repository.Get(_query));
            using (newView.DeferRefresh())
            {
                newView.Culture = _view.Culture;
                foreach (GroupDescription gd in _view.GroupDescriptions)
                    newView.GroupDescriptions.Add(gd);
                foreach (SortDescription sd in _view.SortDescriptions)
                    newView.SortDescriptions.Add(sd);

                newView.CollectionChanged += CollectionChanged;
                newView.CurrentChanged += CurrentChanged;
                newView.CurrentChanging += CurrentChanging;
                newView.PageChanged += PageChanged;
                newView.PageChanging += PageChanging;
                newView.PropertyChanged += PropertyChanged;

                _view = newView;
            }
        }
        public SortDescriptionCollection SortDescriptions
        {
            get { return _view.SortDescriptions; }
        }
        public IEnumerable SourceCollection
        {
            get { return _view.SourceCollection; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _view.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IPagedCollectionView Members

        public bool CanChangePage { get { return _view.CanChangePage; } }
        public bool IsPageChanging { get { return _view.IsPageChanging; } }
        public int ItemCount { get { return _view.ItemCount; } }
        public bool MoveToFirstPage()
        {
            //set the is busy flag
            //set the deferred flag on _view = true
            //fetch from the IRemoteRepository
            //update the SourceCollection on _view
            return _view.MoveToFirstPage();
            //not sure whether to restore the deferred flag here
            //or before the "MoveToPage" call
        }
        public bool MoveToLastPage()
        {
            //set the is busy flag
            //set the deferred flag on _view = true
            //fetch from the IRemoteRepository
            //update the SourceCollection on _view
            return _view.MoveToLastPage();
            //not sure whether to restore the deferred flag here
            //or before the "MoveToPage" call
        }
        public bool MoveToNextPage()
        {
            //set the is busy flag
            //set the deferred flag on _view = true
            //fetch from the IRemoteRepository
            //update the SourceCollection on _view
            return _view.MoveToNextPage();
            //not sure whether to restore the deferred flag here
            //or before the "MoveToPage" call
        }
        public bool MoveToPage(int pageIndex)
        {
            //set the is busy flag
            //set the deferred flag on _view = true
            //fetch from the IRemoteRepository
            //update the SourceCollection on _view
            return _view.MoveToPage(PageIndex);
            //not sure whether to restore the deferred flag here
            //or before the "MoveToPage" call
        }
        public bool MoveToPreviousPage()
        {
            //set the is busy flag
            //set the deferred flag on _view = true
            //fetch from the IRemoteRepository
            //update the SourceCollection on _view
            return _view.MoveToPreviousPage();
            //not sure whether to restore the deferred flag here
            //or before the "MoveToPage" call
        }
        public event EventHandler<EventArgs> PageChanged;
        public event EventHandler<PageChangingEventArgs> PageChanging;
        public int PageIndex { get { return _view.PageIndex; } }
        public int PageSize
        {
            get { return _view.PageSize; }
            set
            {
                //set the is busy flag
                //set the deferred flag on _view = true
                //fetch from the IRemoteRepository
                //update the SourceCollection on _view
                _view.PageSize = value;
                //not sure whether to restore the deferred flag here
                //or before the property is set
            }
        }
        public int TotalItemCount{ get { return _view.TotalItemCount; } }

        #endregion

        #region IEditableCollectionView Members

        public object AddNew()
        {
            return _view.AddNew();
        }
        public bool CanAddNew
        {
            get { return _view.CanAddNew; }
        }
        public bool CanCancelEdit
        {
            get { return _view.CanCancelEdit; }
        }
        public bool CanRemove
        {
            get { return _view.CanRemove; }
        }
        public void CancelEdit()
        {
            _view.CancelEdit();
        }
        public void CancelNew()
        {
            _view.CancelNew();
        }
        public void CommitEdit()
        {
            _view.CommitEdit();
        }
        public void CommitNew()
        {
            _view.CommitNew();
        }
        public object CurrentAddItem
        {
            get { return _view.CurrentAddItem; }
        }
        public object CurrentEditItem
        {
            get { return _view.CurrentEditItem; }
        }
        public void EditItem(object item)
        {
            _view.EditItem(item);
        }
        public bool IsAddingNew
        {
            get { return _view.IsAddingNew; }
        }
        public bool IsEditingItem
        {
            get { return _view.IsEditingItem; }
        }
        public NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get
            {
                return _view.NewItemPlaceholderPosition;
            }
            set
            {
                _view.NewItemPlaceholderPosition = value;
            }
        }
        public void Remove(object item)
        {
            _view.Remove(item);
        }
        public void RemoveAt(int index)
        {
            _view.RemoveAt(index);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
