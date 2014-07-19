namespace CheckbookManager.Data
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.ComponentModel;

    /// <summary>
    /// This is the checkbook register
    /// </summary>
    public class CheckRegisterCollection : ObservableCollection<RegisterTransaction>
    {
        /// <summary>
        /// Hooks each checkbook register item in order to propogate
        /// balance notification.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.NewItems != null)
                e.NewItems
                    .Cast<INotifyPropertyChanged>()
                    .ForEach(inpc => inpc.PropertyChanged += RegisterTransaction_Changed);
            
            if (e.OldItems != null)
                e.OldItems
                    .Cast<INotifyPropertyChanged>()
                    .ForEach(inpc => inpc.PropertyChanged -= RegisterTransaction_Changed);

            // Current balance changed because we've added or removed an entry
            OnPropertyChanged(new PropertyChangedEventArgs("CurrentBalance"));

            // Force a complete recalc of balances - we could optimize this
            // by detecting whether we've added/removed and entry and where it 
            // is but this is much easier.
            this.ForEach(rt => rt.ForceBalanceRefresh());
        }

        /// <summary>
        /// This catches each item's NotifyChange event and recalculates
        /// the effective total balance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterTransaction_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || 
                string.Compare(e.PropertyName, "Amount",true) == 0)
            {
                // Current balance changes
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentBalance"));
                                
                // All total balance properties AFTER this item change
                int pos = IndexOf((RegisterTransaction) sender);
                if (pos >= 0 && pos < Count-1)
                {
                    (from i in Enumerable.Range(pos,Count-pos)
                     select this[i]).ForEach( rt => rt.ForceBalanceRefresh() );
                }
            }
        }

        /// <summary>
        /// Current balance across all transactions.
        /// </summary>
        public double CurrentBalance
        {
            get
            {
                var rt = this.LastOrDefault();
                return (rt == null) ? 0 : BalanceAt(this.Last());
            }   
        }

        /// <summary>
        /// Calculates the current balance from a specific transaction
        /// </summary>
        /// <param name="bt">Transaction</param>
        /// <returns>Money balance</returns>
        public double BalanceAt(RegisterTransaction bt)
        {
            return this.Take(IndexOf(bt)+1).Sum(rt => rt.Amount);
        }
    }
}