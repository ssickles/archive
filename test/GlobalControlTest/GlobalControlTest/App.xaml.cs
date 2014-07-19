using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GlobalControlTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            main.Hide();
            MainWindow2 main2 = new MainWindow2();
            main2.ShowDialog();
            Shutdown();
        }
    }
}
