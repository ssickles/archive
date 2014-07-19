using System;
using System.Windows;
using WickedSick.ClientComponents.Music;

namespace WickedSick.ClientComponents.MusicClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaBI mbi;
        DateTime start;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mbi = new MediaBI("MediaBank.sdb");
            mbi.CommunicationComplete += new CommunicationCompleteDelegate(mbi_CommunicationComplete);
        }

        void mbi_CommunicationComplete(object sender, int CommunicationType)
        {
            if (CommunicationType == 1)
            {
                //Artists, Albums, Songs have been finished transferring and loaded.
                label1.Content = string.Format("{0} seconds", (DateTime.Now - start).Seconds.ToString());
                mbi.SendMediaToFile();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mbi.InitializeMedia(1);
            start = DateTime.Now;
        }
    }
}
