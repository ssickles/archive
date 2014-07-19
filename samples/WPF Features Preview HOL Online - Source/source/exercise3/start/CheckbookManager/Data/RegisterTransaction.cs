namespace CheckbookManager.Data
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// This represents a single transaction in our checkbook register
    /// </summary>
    [DebuggerDisplay("RegisterTransaction [{CheckNumber}]: {Amount} {Recipient} {Description}")]
    public class RegisterTransaction : INotifyPropertyChanged, IEditableObject
    {
        #region Data
        /// <summary>
        /// This class is used to hold all the internal data for
        /// a given transaction using the Pimpl idiom.  We then
        /// swap these around to implement the IEditableObject interface
        /// </summary>
        class RegisterData
        {
            public int? _number;
            public DateTime _date;
            public string _recipient;
            public bool _isCleared;
            public string _description;
            public double _amount;
            public string _category;

            public RegisterData()
            {
            }

            public RegisterData(RegisterData copy)
            {
                _number = copy._number;
                _date = copy._date;
                _recipient = copy._recipient;
                _isCleared = copy._isCleared;
                _description = copy._description;
                _amount = copy._amount;
                _category = copy._category;
            }
        }
        private RegisterData _data = new RegisterData();
        private RegisterData _clone;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        /// <summary>
        /// This initializes a new transaction
        /// </summary>
        public RegisterTransaction()
        {
            Date = DateTime.Today;
        }

        /// <summary>
        /// Date of the transaction
        /// </summary>
        public DateTime Date
        {
            get { return _data._date.Date; }
            set
            {
                _data._date = value.Date;
                OnPropertyChanged("Date");
            }
        }

        /// <summary>
        /// Optional check number of the transaction
        /// </summary>
        public int? CheckNumber
        {
            get { return _data._number; }
            set {
                _data._number = value;
                OnPropertyChanged("CheckNumber"); 
            }
        }

        /// <summary>
        /// Recipient (payee) of the transaction
        /// </summary>
        public string Recipient
        {
            get { return _data._recipient ?? string.Empty; }
            set
            {
                _data._recipient = value;
                OnPropertyChanged("Recipient");
            }
        }

        /// <summary>
        /// Whether this transaction has been cleared/reconciled
        /// </summary>
        public bool Cleared
        {
            get { return _data._isCleared; }
            set
            {
                _data._isCleared = value; 
                OnPropertyChanged("Cleared");
            }
        }

        /// <summary>
        /// Description of the transaction
        /// </summary>
        public string Memo
        {
            get { return _data._description ?? string.Empty; }
            set
            {
                _data._description = value;
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Category of the transaction
        /// </summary>
        public string Category
        {
            get { return _data._category ?? string.Empty; }   
            set
            {
                _data._category = value;
                OnPropertyChanged("Category");
            }
        }

        /// <summary>
        /// Total amount (+/-) of the transaction
        /// </summary>
        public double Amount
        {
            get { return _data._amount; }
            set
            {
                _data._amount = value;
                /* Invalidate entire object as it affects almost everything */
                OnPropertyChanged(string.Empty); 
            }
        }

        /// <summary>
        /// Amount this transaction credits (0 if debit)
        /// </summary>
        public double CreditAmount
        {
            get { return (_data._amount >= 0) ? _data._amount : 0; }
        }

        /// <summary>
        /// Amount this transaction debits (0 if credit)
        /// </summary>
        public double DebitAmount
        {
            get { return (_data._amount < 0) ? _data._amount : 0; }
        }

        /// <summary>
        /// Total balance leading up to this transaction.
        /// Note this property is used specifically for data binding
        /// and should probably be moved off onto a deliberate MV
        /// of this data.
        /// </summary>
        public double TotalBalance
        {
            get { return CheckBook.Register.BalanceAt(this);  } 
            set { /* Required for the DataGrid CTP */ }
        }

        /// <summary>
        /// The collection uses this setter to force
        /// the property to recalculate since it is based
        /// on other elements in the collection
        /// </summary>
        public void ForceBalanceRefresh()
        {
            OnPropertyChanged("TotalBalance");
        }

        #region IEditableObject Members

        /// <summary>
        /// This is called when we are editing the object in a grid
        /// </summary>
        public void BeginEdit()
        {
            _clone = new RegisterData(_data);
        }

        /// <summary>
        /// This is called to cancel edits to the object.  It reverts
        /// the object back to the original (pre-edit) state.
        /// </summary>
        public void CancelEdit()
        {
            _data = _clone;
            _clone = null;

            /* Force entire object to be invalid */
            OnPropertyChanged(string.Empty);
        }

        /// <summary>
        /// This is called to end the edits to the object.  It commits
        /// the changes since BeginEdit.
        /// </summary>
        public void EndEdit()
        {
            _clone = null;
        }

        #endregion
    }
}