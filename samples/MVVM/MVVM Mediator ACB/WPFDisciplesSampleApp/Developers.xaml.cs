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
using WPFDisciples.ViewModels;
using System.Reflection;

namespace WPFDisciples
{
    /// <summary>
    /// Interaction logic for Developers.xaml
    /// </summary>
    public partial class Developers : UserControl
    {
        public Developers()
        {
            InitializeComponent();
            DataContext = new DevelopersViewModel(Dispatcher);
        }
    }
}
