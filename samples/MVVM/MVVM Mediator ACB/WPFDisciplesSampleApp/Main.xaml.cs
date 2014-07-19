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
using WPFDisciples.ViewModels;

namespace WPFDisciples
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();

            var vm = new MainViewModel();
            vm.CurrentView = "listProjects";//set the default view to be the listProjects
            DataContext = vm;

            //Handle dragging of the window since we removed the default TitleBar
            MouseLeftButtonDown += delegate { DragMove(); };
            //handle the close command
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, delegate { Close(); }));
        }

    }
}
