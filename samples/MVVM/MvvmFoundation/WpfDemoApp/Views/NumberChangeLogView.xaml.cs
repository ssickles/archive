using System;
using System.Windows.Controls;

namespace WpfDemoApp.Views
{
    public partial class NumberChangeLogView : UserControl
    {
        public NumberChangeLogView()
        {
            InitializeComponent();

            App.Messenger.Register(
                App.MSG_LOG_APPENDED,
                new Action(() => _scrollViewer.ScrollToBottom()));
        }
    }
}