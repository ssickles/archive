/// Notice : Code written by Dimitris Papadimitriou - http://www.papadi.gr
/// Code is provided to be used freely but without any warranty of any kind
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());
        }
    }
}