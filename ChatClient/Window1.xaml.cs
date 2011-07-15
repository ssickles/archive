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
using System.ServiceModel;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, MessagingService.IChatCallback
    {
        private Guid _myId;
        MessagingService.ChatClient _client;

        public Window1()
        {
            InitializeComponent();

            _myId = Guid.NewGuid();

            _client = new MessagingService.ChatClient(new InstanceContext(this));
            _client.Subscribe();
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            _client.PostMessage(_myId, Input.Text);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            _client.Unsubscribe();
        }

        #region IChatCallback Members

        public void MessagePosted(Guid ClientId, string Message)
        {
            if (!ClientId.Equals(_myId))
                Messages.Text += Message + Environment.NewLine;
        }

        public IAsyncResult BeginMessagePosted(Guid ClientId, string Message, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndMessagePosted(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
