using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Navigation;
using SHAppBarMessage1.Common;

namespace WPFAppBarMessage1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {

        public Window1()
        {
            InitializeComponent();

            helper = new WindowInteropHelper(this);

        }

        private void OnWindowInitialized(object sender, EventArgs e)
        {
            Visibility = Visibility.Visible;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {

            //WindowInteropHelper helper = new WindowInteropHelper(this);
            handle = helper.Handle;
            System.Drawing.Size size = new System.Drawing.Size((int)this.ActualWidth, (int)this.ActualHeight);

            SHAppBarMessageHelper.RegisterBar(handle, size);
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            //WindowInteropHelper helper = new WindowInteropHelper(this);
            System.Drawing.Size size = new System.Drawing.Size((int)this.ActualWidth, (int)this.ActualHeight);

            SHAppBarMessageHelper.RegisterBar(handle, size);
        }

        private WindowInteropHelper helper;
        private IntPtr handle;
    }
}