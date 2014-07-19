using System.Windows;
using MvvmFoundation.Wpf;

namespace WpfDemoApp
{
    public partial class App : Application
    {
        internal static Messenger Messenger
        {
            get { return _messenger; }
        }

        readonly static Messenger _messenger = new Messenger();

        internal const string MSG_LOG_APPENDED = "Log Appended";
    }
}