using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using WickedSick.CommonComponents.IO;
using WickedSick.Clients.FileSync.Properties;
using WickedSick.CommonComponents.Toolbox;

namespace WickedSick.Clients.FileSync
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private SysTray _sysTray;
        private FileSyncManager _syncManager;

        public Main()
        {
            InitializeComponent();

            _syncManager = new FileSyncManager();
            foreach (string loc in Settings.Default.Locations)
            {
                _syncManager.AddLocation(loc);
            }

            _sysTray = new SysTray(this, new Icon("../../Sync.ico"), (System.Windows.Controls.ContextMenu)FindResource("NotifyContext"));

            SyncList.ItemsSource = _syncManager.Locations;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                _sysTray.HideWindow();
            }

            base.OnStateChanged(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _sysTray.Dispose();

            base.OnClosing(e);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            _sysTray.ShowWindow();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select Path";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _syncManager.AddLocation(dialog.SelectedPath);
            }
        }
    }
}
