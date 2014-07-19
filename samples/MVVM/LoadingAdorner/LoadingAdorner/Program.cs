using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LoadingAdorner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application app = new Application();
            app.Run(new DemoPage());
        }
    }
}
