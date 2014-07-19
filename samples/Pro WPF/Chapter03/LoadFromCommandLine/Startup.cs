using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;

namespace LoadFromCommandLine
{
    public class Startup
    {
        [STAThread()]
        static void Main()
        {
            // Create the application and register for the Startup event.
            Application app = new Application(); 
            app.Startup += App_Startup;

            // Launch the application
            app.Run(new FileViewer());
        }

        // The command-line argument is set through the Visual Studio
        // project properties (the Debug tab).
        private static void App_Startup(object sender, StartupEventArgs e)
        {
            // At this point, the main window has been set but not shown.
            if (e.Args.Length > 0)
            {
                string file = e.Args[0];
                if (File.Exists(file))
                {
                    // Configure the main window.
                    FileViewer win = (FileViewer)Application.Current.MainWindow;
                    win.LoadFile(file);
                }
            }
        }
    }

}
