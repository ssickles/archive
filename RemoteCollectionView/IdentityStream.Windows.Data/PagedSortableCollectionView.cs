// Copyright (c) Manish Dalal, 2008-2009. All Rights Reserved.
// http://weblogs.asp.net/manishdalal
//
// This product's copyrights are licensed under 
// the CreativeCommons Attribution-ShareAlike (version 3)
// http://creativecommons.org/licenses/by-sa/3.0/

using System;
using System.ComponentModel;

namespace SilverlightApplication.CustomSort {
    /// <summary>
    /// Represents the paged sortable collection view
    /// </summary>
    /// <typeparam name="T">Type of Item in the collection</typeparam>
    public class PagedSortableCollectionView<T> : SortableCollectionView<T>, IPagedCollectionView {

        #region IPagedCollectionView Members

        /// <summary>
        /// Gets a value that indicates whether the <see cref="P:System.ComponentModel.IPagedCollectionView.PageIndex"/> value can change.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="P:System.ComponentModel.IPagedCollectionView.PageIndex"/> value can change; otherwise, false.</returns>
        public bool CanChangePage {
            get { return true; ; }
        }

        private bool _isPageChanging;
        /// <summary>
        /// Gets a value that indicates whether the page index is changing.
        /// </summary>
        /// <value></value>
        /// <returns>true if the page index is changing; otherwise, false.</returns>
        public bool IsPageChanging {
            get { return _isPageChanging; }
            private set {
                if (_isPageChanging != value) {
                    _isPageChanging = value;
                    OnPropertyChanged("IsPageChanging");
                }
            }
        }

        /// <summary>
        /// Gets the number of known items in the view before paging is applied.
        /// </summary>
        /// <value></value>
        /// <returns>The number of known items in the view before paging is applied.</returns>
        public int ItemCount {
            get {
                return TotalItemCount;
            }
            set {
                TotalItemCount = value;
            }
        }

        /// <summary>
        /// Sets the first page as the current page.
        /// </summary>
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        public bool MoveToFirstPage() {
            return this.MoveToPage(0);
        }

        /// <summary>
        /// Sets the last page as the current page.
        /// </summary>
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        public bool MoveToLastPage() {
            return (((this.TotalItemCount != -1) && (this.PageSize > 0)) && this.MoveToPage(this.PageCount - 1));
        }

        /// <summary>
        /// Moves to the page after the current page.
        /// </summary>
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        public bool MoveToNextPage() {
            return MoveToPage(_pageIndex + 1);
        }

        /// <summary>
        /// Moves to the page at the specified index.
        /// </summary>
        /// <param name="pageIndex">The index of the page to move to.</param>
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        public bool MoveToPage(int pageIndex) {
            if (pageIndex < -1) {
                return false;
            }
            if ((pageIndex == -1) && (this.PageSize > 0)) {
                return false;
            }
            if ((pageIndex >= this.PageCount) || (this._pageIndex == pageIndex)) {
                return false;
            }
            //
            try {
                IsPageChanging = true;
                if (null != PageChanging) {
                    PageChangingEventArgs args = new PageChangingEventArgs(pageIndex);
                    OnPageChanging(args);
                    if (args.Cancel) return false;
                }
                //
                _pageIndex = pageIndex;
                Refresh();
                IsPageChanging = false;
                OnPropertyChanged("PageIndex");
                OnPageChanged(EventArgs.Empty);
                return true;
            } finally {
                IsPageChanging = false;
            }
        }

        /// <summary>
        /// Moves to the page before the current page.
        /// </summary>
        /// <returns>
        /// true if the operation was successful; otherwise, false.
        /// </returns>
        public bool MoveToPreviousPage() {
            return MoveToPage(_pageIndex - 1);
        }

        /// <summary>
        /// When implementing this interface, raise this event after the <see cref="P:System.ComponentModel.IPagedCollectionView.PageIndex"/> has changed.
        /// </summary>
        public event EventHandler<EventArgs> PageChanged;

        /// <summary>
        /// When implementing this interface, raise this event before changing the <see cref="P:System.ComponentModel.IPagedCollectionView.PageIndex"/>. The event handler can cancel this event.
        /// </summary>
        public event EventHandler<PageChangingEventArgs> PageChanging;

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount {
            get {
                if (this._pageSize <= 0) {
                    return 0;
                }
                return Math.Max(1, (int)Math.Ceiling(((double)this.ItemCount) / ((double)this._pageSize)));

            }
        }

        private int _pageIndex;
        /// <summary>
        /// Gets the zero-based index of the current page.
        /// </summary>
        /// <value></value>
        /// <returns>The zero-based index of the current page.</returns>
        public int PageIndex {
            get {
                return _pageIndex;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PageChanging"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.ComponentModel.PageChangingEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPageChanging(PageChangingEventArgs args) {
            if (null != PageChanging) {
                PageChanging(this, args);
            }
        }
        /// <summary>
        /// Raises the <see cref="E:PageChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPageChanged(EventArgs args) {
            if (null != PageChanged) {
                PageChanged(this, args);
            }
        }

        /// <summary>
        /// defaults to 10 rows per page
        /// </summary>
        private int _pageSize = 10;
        /// <summary>
        /// Gets or sets the number of items to display on a page.
        /// </summary>
        /// <value></value>
        /// <returns>The number of items to display on a page.</returns>
        public int PageSize {
            get {
                return _pageSize;
            }
            set {
                if (_pageSize != value && value >= 1) {
                    _pageSize = value;
                    OnPropertyChanged("PageSize");
                }
            }
        }

        private int _totalItemCount;
        /// <summary>
        /// Gets the total number of items in the view before paging is applied.
        /// </summary>
        /// <value></value>
        /// <returns>The total number of items in the view before paging is applied, or -1 if the total number is unknown.</returns>
        public int TotalItemCount {
            get {
                return _totalItemCount;
            }
            set {
                if (_totalItemCount != value) {
                    _totalItemCount = value;
                    OnPropertyChanged("TotalItemCount");
                    OnPropertyChanged("ItemCount");
                }
            }
        }

        #endregion

    }
}
