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
using Neurotec.Biometrics;
using System.Diagnostics;

namespace NeuroSDK
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Sync _sync;
        private FPScannerMan _scannerMan;

        public Window1()
        {
            InitializeComponent();

            _sync = new Sync();
            _scannerMan = new FPScannerMan(_sync);
            _scannerMan.ScannerAdded += new FPScannerManScannerEventHandler(_scannerMan_ScannerAdded);
            _scannerMan.ScannerRemoved += new FPScannerManScannerEventHandler(_scannerMan_ScannerRemoved);
        }

        void _scannerMan_ScannerRemoved(object sender, FPScannerManScannerEventArgs e)
        {
            Debug.WriteLine(string.Format("Scanner Removed - {0}", e.Scanner.ID));
        }

        void _scannerMan_ScannerAdded(object sender, FPScannerManScannerEventArgs e)
        {
            Debug.WriteLine(string.Format("Scanner Added - {0}", e.Scanner.ID));
        }
    }
}
