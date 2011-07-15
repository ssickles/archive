using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SecuredWcfService
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
    public class SecuredService : ISecuredService
    {
        private bool _isLoggedIn = false;
        private string _username = string.Empty;

        public bool Login(string Username, string Password)
        {
            if (Username.ToLower() == "scott" || Username.ToLower() == "brian" || Username.ToLower() == "james")
            {
                _username = Username;
                _isLoggedIn = true;
                return true;
            }
            else
            {
                throw new FaultException<ErrorDetails>(new ErrorDetails("Login", DateTime.Now.ToString()));
            }
        }

        public string GetLoggedInUser()
        {
            if (_isLoggedIn)
                return _username;
            else
                throw new FaultException<ErrorDetails>(new ErrorDetails("GetLoggedInUser", DateTime.Now.ToString()));
        }

        public bool IsLoggedIn()
        {
            if (_isLoggedIn)
                return true;
            else
                throw new FaultException<ErrorDetails>(new ErrorDetails("IsLoggedIn", DateTime.Now.ToString()));
        }
    }
}
