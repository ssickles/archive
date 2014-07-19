using System.Windows;
using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.ServiceModel;

namespace NeoIntegrationSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon _trayIcon;
        private System.Windows.Forms.ContextMenu _trayContextMenu;
        private Main _mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //build the context menu
            _trayContextMenu = new System.Windows.Forms.ContextMenu();
            _trayContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("E&xit", ContextMenuExit));

            //setup the system tray icon
            _trayIcon = new NotifyIcon();
            _trayIcon.ContextMenu = _trayContextMenu;
            _trayIcon.Icon = images.IDS04;
            _trayIcon.Click += new EventHandler(TrayIconClick);
            _trayIcon.Visible = true;

            //host the service
            ServiceHost host1 = new ServiceHost(typeof(CrossDomainService));
            host1.Open();
            ServiceHost host2 = new ServiceHost(typeof(BiometricsService));
            host2.Open();

            //display the main window
            _mainWindow = new Main();
            _mainWindow.Closing += new CancelEventHandler(MainWindowClosing);
            _mainWindow.ShowDialog();

            //cleanup
            _trayContextMenu.Dispose();
            _trayIcon.Dispose();
        }

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            _mainWindow.ShowInTaskbar = false;
            _mainWindow.WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        private void TrayIconClick(object sender, EventArgs e)
        {
            _mainWindow.ShowInTaskbar = true;
            _mainWindow.WindowState = WindowState.Normal;
        }

        private void ContextMenuExit(object sender, EventArgs e)
        {
            Shutdown();
        }
    }
}
