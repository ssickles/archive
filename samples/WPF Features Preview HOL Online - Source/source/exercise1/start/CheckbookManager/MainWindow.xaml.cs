using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CheckbookManager.Data;

namespace CheckbookManager
{
    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private const string filename = @"checkbook.xml";

        public MainWindow()
        {
            CheckBook.Load(filename);

            // Fill with data if no file present.
            if (CheckBook.Register.Count == 0)
            {
                CheckBook.Register.Add(new RegisterTransaction { Date = new DateTime(2008,10,01,0,0,0), Amount = 1000, Recipient = "Counter Credit", Cleared = true });
                CheckBook.Register.Add(new RegisterTransaction { Date = new DateTime(2008,10,02,0,0,0), Amount = -599.99, Recipient = "Microsoft", Memo = "MSDN Universal Subscription License", CheckNumber = 501, Cleared = true });
                CheckBook.Register.Add(new RegisterTransaction { Date = new DateTime(2008,10,04,0,0,0), Amount = -50.99, Recipient = "Chillis", Memo = "Lunch with Dave", Category = "Dining Out", CheckNumber = 502, Cleared = true });
                CheckBook.Register.Add(new RegisterTransaction { Date = new DateTime(2008,10,05,0,0,0), Amount = 543.33, Recipient = "Deposit", Memo = "Check from Bill for Dad's b-day gift", Category = "Deposit" });
                CheckBook.Register.Add(new RegisterTransaction { Date = new DateTime(2008,10,06,0,0,0), Amount = -2795, Recipient = "Micro Center", Memo = "New 30\" LCD", Category="Computers: Hardware", CheckNumber = 503 });
                CheckBook.Register.Add(new RegisterTransaction { Date = new DateTime(2008,10,06,0,0,0), Amount = 1000, Recipient = "Deposit", Memo = "Transfer from Savings", Category = "Deposit" });
            }

            // Set the data context to our view model
            DataContext = CheckBook.Register;

            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            CheckBook.Save(filename);
            base.OnClosed(e);
        }

        private void OnCloseApplication(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void OnAddNewEntry(object sender, ExecutedRoutedEventArgs e)
        {
            var ri = new RegisterTransaction();
            CheckBook.Register.Add(ri);
        }

        private void OnClearEntry(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (RegisterTransaction rt in dg.SelectedItems)
                rt.Cleared = true;
        }

        private void OnDeleteEntry(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(
                string.Format("Are you sure you want to delete {0}?",
                                dg.SelectedItems.Count > 1 
                                    ? string.Format("{0} entries?", dg.SelectedItems.Count)
                                    : "this entry"),
                "Verify your request", MessageBoxButton.YesNo) == MessageBoxResult.Yes )
            {
                foreach (var rt in dg.SelectedItems.OfType<RegisterTransaction>().ToArray())
                {
                    CheckBook.Register.Remove(rt);
                }
            }
        }

        private void OnHasSelectedEntries(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (dg != null && dg.SelectedItem as RegisterTransaction != null);
        }

        private void OnIgnore(object sender, ExecutedRoutedEventArgs e)
        {
        }
    }
}
