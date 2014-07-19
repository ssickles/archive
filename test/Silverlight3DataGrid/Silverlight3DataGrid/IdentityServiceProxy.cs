using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Silverlight3DataGrid.IdentityServiceReference;
using System.ServiceModel.Channels;
using System.ComponentModel;
using System.Windows.Browser;
using System.ServiceModel;

namespace Silverlight3DataGrid
{
    public class IdentityServiceProxy : INotifyPropertyChanged
    {
        private bool _IsLoggedIn;

        static IdentityServiceProxy()
        {
            Service = new IdentityServiceClient(BuildCustomBinding(false), BuildEndpoint());
            Instance = new IdentityServiceProxy();
        }

        public static IdentityServiceClient Service { get; private set; }

        public static IdentityServiceProxy Instance { get; private set; }

        internal static CustomBinding BuildCustomBinding(bool isSsl)
        {
            BinaryMessageEncodingBindingElement binary = new BinaryMessageEncodingBindingElement();
            HttpTransportBindingElement transport = isSsl ? new HttpsTransportBindingElement() : new HttpTransportBindingElement();
            transport.MaxBufferSize = 2147483647;
            transport.MaxReceivedMessageSize = 2147483647;
            return new CustomBinding(binary, transport);
        }

        internal static EndpointAddress BuildEndpoint()
        {
            string host = HtmlPage.Document.DocumentUri.Host;
            if (HtmlPage.Document.DocumentUri.Port != 80)
            {
                host += string.Format(":{0}", HtmlPage.Document.DocumentUri.Port);
            }
            return new EndpointAddress(
                string.Format("http://{0}/TestClient/IdentityService.svc", host));
        }

        public bool IsLoggedIn
        {
            get { return _IsLoggedIn; }
            set
            {
                if (_IsLoggedIn != value)
                {
                    _IsLoggedIn = value;
                    RaisePropertyChanged("IsLoggedOn");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler obj = PropertyChanged;
            if (obj != null)
            {
                obj(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
