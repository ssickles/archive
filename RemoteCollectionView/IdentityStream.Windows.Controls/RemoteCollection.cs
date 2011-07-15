using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using IdentityStream.Data;
using System.Globalization;
using System.Diagnostics;

namespace IdentityStream.Windows.Controls
{
    public class RemoteCollection<T>: ObservableCollection<T>, ICollectionView, IPagedCollectionView
    {
        private CultureInfo _culture;
        private int _currentPosition;
        private bool _isEmpty;
        private Predicate<object> _filter;

        private Func<QueryObject, int, List<T>> _fetch;

        public RemoteCollection(Func<QueryObject, int, List<T>> fetchRoutine)
            : base()
        {
            _culture = CultureInfo.CurrentUICulture;
            _currentPosition = -1;
            _isEmpty = true;
            _filter = new Predicate<object>(delegate(object o) { Debug.WriteLine("Execute Filter"); return true; });

            _fetch = fetchRoutine;
        }

        #region ICollectionView Members

        public bool CanFilter
        {
            get { return true; }
        }
        public bool CanGroup
        {
            get { return true; }
        }
        public bool CanSort
        {
            get { return true; }
        }
        public bool Contains(object item)
        {
            return base.Contains((T)item);
        }
        public CultureInfo Culture
        {
            get
            {
                return _culture;
            }
            set
            {
                if (_culture != value)
                {
                    _culture = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Culture"));
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
                if (!IsEmpty && CurrentPosition >= 0 && CurrentPosition < Count)
                    return this[CurrentPosition];
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
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrentPosition"));
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrentItem"));
                    OnCurrentChanged();
                }
            }
        }
        public IDisposable DeferRefresh()
        {
            throw new NotImplementedException();
        }
        public Predicate<object> Filter
        {
            get
            {
                return _filter;
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
                    OnPropertyChanged(new PropertyChangedEventArgs("IsEmpty"));
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
                for (int i = 0; i < Count; i++)
                {
                    if (this[i].Equals(castItem))
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
                CurrentPosition = Count - 1;
                return true;
            }
        }
        public bool MoveCurrentToNext()
        {
            if (CurrentPosition < Count - 1)
            {
                CurrentPosition++;
                return true;
            }
            else
                return false;
        }
        public bool MoveCurrentToPosition(int position)
        {
            if (position >= 0 && position < Count - 1)
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
            throw new NotImplementedException();
        }
        public System.ComponentModel.SortDescriptionCollection SortDescriptions
        {
            get { throw new NotImplementedException(); }
        }
        public System.Collections.IEnumerable SourceCollection
        {
            get { return this; }
        }

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
    }
}
