namespace CheckbookManager
{
    using System.Windows.Input;

    /// <summary>
    /// This class holds the global commands used by the application
    /// </summary>
    public static class AppCommands
    {
        public static RoutedCommand Help = new RoutedUICommand("Help", "Help", typeof (AppCommands));
        public static RoutedCommand AddNew = new RoutedUICommand("Add New Entry", "AddNew", typeof (AppCommands));
        public static RoutedCommand Clear = new RoutedUICommand("Mark Cleared", "Clear", typeof (AppCommands));
        public static RoutedCommand Delete = new RoutedUICommand("Delete", "Delete", typeof (AppCommands));
        public static RoutedCommand Reconcile = new RoutedUICommand("Reconcile", "Reconcile", typeof(AppCommands));
        public static RoutedCommand CashflowReport = new RoutedUICommand("Cashflow Report", "CashflowReport", typeof(AppCommands));
        public static RoutedCommand BudgetReport = new RoutedUICommand("Budget Report", "BudgetReport", typeof(AppCommands));
        public static RoutedCommand TrendReport = new RoutedUICommand("Trend Report", "TrendReport", typeof(AppCommands));
        public static RoutedCommand OtherReports = new RoutedUICommand("Other Reports", "OtherReports", typeof(AppCommands));
        public static RoutedCommand DownloadStatements = new RoutedUICommand("Download Statements", "DownloadStatements", typeof(AppCommands));
        public static RoutedCommand DownloadCreditCards = new RoutedUICommand("Download CreditCards", "DownloadCreditCards", typeof(AppCommands));
        public static RoutedCommand Transfer = new RoutedUICommand("Transfer", "Transfer", typeof(AppCommands));
        public static RoutedCommand Backup = new RoutedUICommand("Backup", "Backup", typeof(AppCommands));
        public static RoutedCommand Calculator = new RoutedUICommand("Calculator", "Calculator", typeof(AppCommands));
    }
}
