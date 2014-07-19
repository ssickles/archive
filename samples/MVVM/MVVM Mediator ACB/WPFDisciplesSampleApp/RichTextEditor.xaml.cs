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

namespace WPFDisciples
{
    /// <summary>
    /// Interaction logic for RichTextEditor.xaml
    /// </summary>
    public partial class RichTextEditor : UserControl
    {
        public RichTextEditor()
        {
            InitializeComponent();
            DataContext = new RichTextViewModel();
        }
    }
}
