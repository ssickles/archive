using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ChatService
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatClient))]
    interface IChat
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe(Guid ClientId);
        [OperationContract(IsOneWay = false, IsTerminating = true)]
        void Unsubscribe();
        [OperationContract(IsOneWay = true)]
        void PostMessage(string Message);
    }

    interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void MessagePosted(string Message);
    }
}
