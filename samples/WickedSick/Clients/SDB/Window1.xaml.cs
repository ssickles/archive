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
using System.Windows.Shapes;
using WickedSick.ClientComponents.Music;

namespace SDB
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
        private MediaBI media;

		public Window1()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.

            this.Loaded += new RoutedEventHandler(Window1_Loaded);
		}

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            media = new MediaBI("media.sdb");
            media.CommunicationComplete += new CommunicationCompleteDelegate(media_CommunicationComplete);
            media.InitializeMedia(1);
        }

        void media_CommunicationComplete(object sender, int CommunicationType)
        {
            DataDisplay.ItemsSource = media.Bank.Artists.Values;
        }
	}
}