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

        //#region RibbonCommands
        //public static RibbonCommand Help
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["HelpCommand"]; }
        //}
        //public static RibbonCommand Cut
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["CutCommand"]; }
        //}
        //public static RibbonCommand Copy
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["CopyCommand"]; }
        //}
        //public static RibbonCommand Paste
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["PasteCommand"]; }
        //}
        //public static RibbonCommand AddNew
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["AddNewCommand"]; }
        //}
        //public static RibbonCommand Clear
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["ClearCommand"]; }
        //}
        //public static RibbonCommand Delete
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["DeleteCommand"]; }
        //}
        //public static RibbonCommand Reconcile
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["ReconcileCommand"]; }
        //}
        //public static RibbonCommand CashflowReport
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["CashflowReportCommand"]; }
        //}
        //public static RibbonCommand BudgetReport
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["BudgetReportCommand"]; }
        //}
        //public static RibbonCommand TrendReport
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["TrendReportCommand"]; }
        //}
        //public static RibbonCommand OtherReports
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["OtherReportsCommand"]; }
        //}
        //public static RibbonCommand DownloadStatements
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["DownloadStatementsCommand"]; }
        //}
        //public static RibbonCommand DownloadCreditCards
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["DownloadCreditCardsCommand"]; }
        //}
        //public static RibbonCommand Transfer
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["TransferCommand"]; }
        //}
        //public static RibbonCommand Backup
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["BackupCommand"]; }
        //}
        //public static RibbonCommand Calculator
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["CalculatorCommand"]; }
        //}
        //#endregion
    }
}
