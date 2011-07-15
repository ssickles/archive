using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using IdentityStream.Data;
using System.Globalization;
using System.Collections.ObjectModel;

namespace IdentityStream.Windows.Controls
{
    public sealed class RemoteCollectionView2 : ICollectionView, IEnumerable, INotifyCollectionChanged, IPagedCollectionView, IEditableCollectionView, INotifyPropertyChanged
    {
        private IRemoteRepository _repository;

        public RemoteCollectionView2(IRemoteRepository repository)
        {
            _repository = repository;
        }

        #region ICollectionView Members

        public bool CanFilter
        {
            get { throw new NotImplementedException(); }
        }
        public bool CanGroup
        {
            get { throw new NotImplementedException(); }
        }
        public bool CanSort
        {
            get { throw new NotImplementedException(); }
        }
        public bool Contains(object item)
        {
            throw new NotImplementedException();
        }
        public CultureInfo Culture
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
        public event EventHandler CurrentChanged;
        public event CurrentChangingEventHandler CurrentChanging;
        public object CurrentItem
        {
            get { throw new NotImplementedException(); }
        }
        public int CurrentPosition
        {
            get { throw new NotImplementedException(); }
        }
        public IDisposable DeferRefresh()
        {
            throw new NotImplementedException();
        }
        public Predicate<object> Filter
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
            get { throw new NotImplementedException(); }
        }
        public ReadOnlyObservableCollection<object> Groups
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsCurrentAfterLast
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsCurrentBeforeFirst
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsEmpty
        {
            get { throw new NotImplementedException(); }
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
            throw new NotImplementedException();
        }
        public SortDescriptionCollection SortDescriptions
        {
            get { throw new NotImplementedException(); }
        }
        public IEnumerable SourceCollection
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IPagedCollectionView Members

        public bool CanChangePage
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsPageChanging
        {
            get { throw new NotImplementedException(); }
        }

        public int ItemCount
        {
            get { throw new NotImplementedException(); }
        }

        public int PageIndex
        {
            get { throw new NotImplementedException(); }
        }

        public int PageSize
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

        public int TotalItemCount
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> PageChanged;

        public event EventHandler<PageChangingEventArgs> PageChanging;

        public bool MoveToFirstPage()
        {
            throw new NotImplementedException();
        }

        public bool MoveToLastPage()
        {
            throw new NotImplementedException();
        }

        public bool MoveToNextPage()
        {
            throw new NotImplementedException();
        }

        public bool MoveToPage(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public bool MoveToPreviousPage()
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
