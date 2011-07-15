using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using IdentityStream.Client.Biometrics;

namespace CustomerSearch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Application.Current.Exit += new ExitEventHandler(Current_Exit);
            Application.Current.Properties["Dispatcher"] = Dispatcher;
            FingerprintCapture.Create();
            Search search = new Search();
            search.ShowDialog();
            this.Shutdown();
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            FingerprintCapture.Destroy();
        }
    }
}
