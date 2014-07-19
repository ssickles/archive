using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Resources;
using System.Collections;

namespace ResourceReaderWriter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ResourceReader reader = new ResourceReader("C:\\Users\\ssickles\\Desktop\\Infralution.Localization.Wpf\\SampleApp_CS\\bin\\Debug\\fr\\WpfApp.resources.dll");
            IDictionaryEnumerator resourceReaderEn = reader.GetEnumerator();
            while (resourceReaderEn.MoveNext())
            {
                Console.WriteLine("Name: {0} - Value: {1}",
                resourceReaderEn.Key.ToString().PadRight(10, ' '),
                resourceReaderEn.Value);
            }
            ResourceWriter writer = new ResourceWriter("C:\\Users\\ssickles\\Desktop\\Infralution.Localization.Wpf\\SampleApp_CS\\bin\\Debug\\fr\\WpfApp.resources.dll");
        }
    }
}
