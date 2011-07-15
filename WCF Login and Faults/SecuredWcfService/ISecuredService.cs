using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SecuredWcfService
{
    [ServiceContract(SessionMode=SessionMode.Required)]
    public interface ISecuredService
    {
        [OperationContract]
        [FaultContract(typeof(ErrorDetails))]
        bool Login(string Username, string Password);
        [OperationContract]
        [FaultContract(typeof(ErrorDetails))]
        string GetLoggedInUser();
        [OperationContract]
        [FaultContract(typeof(ErrorDetails))]
        bool IsLoggedIn();
    }
}
