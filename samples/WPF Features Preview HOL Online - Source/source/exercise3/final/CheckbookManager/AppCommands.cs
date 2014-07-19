namespace CheckbookManager
{
    using Microsoft.Windows.Controls.Ribbon;
    using System.Windows;

    /// <summary>
    /// This class holds the global commands used by the application
    /// </summary>
    public static class AppCommands
    {
        public static RibbonCommand Help
        {
            get { return (RibbonCommand)Application.Current.Resources["HelpCommand"]; }
        }
        public static RibbonCommand Cut
        {
            get { return (RibbonCommand)Application.Current.Resources["CutCommand"]; }
        }
        public static RibbonCommand Copy
        {
            get { return (RibbonCommand)Application.Current.Resources["CopyCommand"]; }
        }
        public static RibbonCommand Paste
        {
            get { return (RibbonCommand)Application.Current.Resources["PasteCommand"]; }
        }
        public static RibbonCommand AddNew
        {
            get { return (RibbonCommand)Application.Current.Resources["AddNewCommand"]; }
        }
        public static RibbonCommand Clear
        {
            get { return (RibbonCommand)Application.Current.Resources["ClearCommand"]; }
        }
        public static RibbonCommand Delete
        {
            get { return (RibbonCommand)Application.Current.Resources["DeleteCommand"]; }
        }
        public static RibbonCommand Reconcile
        {
            get { return (RibbonCommand)Application.Current.Resources["ReconcileCommand"]; }
        }
        public static RibbonCommand CashflowReport
        {
            get { return (RibbonCommand)Application.Current.Resources["CashflowReportCommand"]; }
        }
        public static RibbonCommand BudgetReport
        {
            get { return (RibbonCommand)Application.Current.Resources["BudgetReportCommand"]; }
        }
        public static RibbonCommand TrendReport
        {
            get { return (RibbonCommand)Application.Current.Resources["TrendReportCommand"]; }
        }
        public static RibbonCommand OtherReports
        {
            get { return (RibbonCommand)Application.Current.Resources["OtherReportsCommand"]; }
        }
        public static RibbonCommand DownloadStatements
        {
            get { return (RibbonCommand)Application.Current.Resources["DownloadStatementsCommand"]; }
        }
        public static RibbonCommand DownloadCreditCards
        {
            get { return (RibbonCommand)Application.Current.Resources["DownloadCreditCardsCommand"]; }
        }
        public static RibbonCommand Transfer
        {
            get { return (RibbonCommand)Application.Current.Resources["TransferCommand"]; }
        }
        public static RibbonCommand Backup
        {
            get { return (RibbonCommand)Application.Current.Resources["BackupCommand"]; }
        }
        public static RibbonCommand Calculator
        {
            get { return (RibbonCommand)Application.Current.Resources["CalculatorCommand"]; }
        }
    }
}
