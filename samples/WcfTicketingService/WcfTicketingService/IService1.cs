using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfTicketingService
{
    [ServiceContract(
    Name = "TicketingService",
    Namespace = "http://www.learn2develop.net/",
    CallbackContract = typeof(ITicketCallBack),
    SessionMode = SessionMode.Required)]
    public interface ITicketService
    {
        [OperationContract(IsOneWay = true)]
        void SetSeatStatus(string strSeats);

        [OperationContract(IsOneWay = true)]
        void RegisterClient(Guid id);

        [OperationContract(IsOneWay = true)]
        void UnRegisterClient(Guid id);
    }

    public interface ITicketCallBack
    {
        [OperationContract(IsOneWay = true)]
        void SeatStatus(string message);
    }

    //---each client connected to the service has a GUID---
    [DataContract]
    public class Client
    {
        [DataMember]
        public Guid id { get; set; }
    }
}
