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
using IdentityStream.BioAPI;
using System.ComponentModel;

namespace BioAPIWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        BackgroundWorker bioCapture;

        public Window1()
        {
            InitializeComponent();

            bioCapture = new BackgroundWorker();
            bioCapture.DoWork += new DoWorkEventHandler(bioCapture_DoWork);
            bioCapture.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bioCapture_RunWorkerCompleted);
        }

        private void bioCapture_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(string.Format("Template Captured: {0}", ((Template)e.Result).MinutiaCount));
            Capture.IsEnabled = true;
        }

        private void bioCapture_DoWork(object sender, DoWorkEventArgs e)
        {
            BiometricContext.CaptureBSPs = new List<Type>() { Type.GetType("IdentityStream.BSPs.Capture.Test.TestCapture,IdentityStream.BSPs.Capture.Test") };
            CaptureSession session = BiometricContext.Current.CreateCaptureSession();
            Template template = session.Capture(1);
            BiometricContext.Current.EndCaptureSession();
            e.Result = template;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Capture.IsEnabled = false;
            bioCapture.RunWorkerAsync();
        }
    }
}
