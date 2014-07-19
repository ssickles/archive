using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

//using System.Collections;
namespace WcfTicketingService
{
    [ServiceBehavior(InstanceContextMode =
        InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Ticketing : ITicketService
    {
        //---used to represent all the seats---
        //bool[,] seatsArray = new bool[10, 10];

        //---used for locking---
        private object locker = new object();

        private SeatStatus _seatStatus = null;

        //---for storing all the clients connected to the service---
        private Dictionary<Client, ITicketCallBack> clients =
            new Dictionary<Client, ITicketCallBack>();

        public Ticketing() { }

        //---add a newly connected client to the dictionary---
        public void RegisterClient(Guid guid)
        {
            ITicketCallBack callback =
                OperationContext.Current.GetCallbackChannel
                <ITicketCallBack>();

            //---prevent multiple clients adding at the same time---
            lock (locker)
            {
                clients.Add(new Client { id = guid }, callback);
            }
        }

        //---unregister a client by removing its GUID from 
        // dictionary---
        public void UnRegisterClient(Guid guid)
        {
            var query = from c in clients.Keys
                        where c.id == guid
                        select c;
            clients.Remove(query.First());
        }

        //---called by clients when they want to book seats---
        public void SetSeatStatus(string strSeats)
        {
            _seatStatus = new SeatStatus
            {
                //---stores the seats to be booked by a client---
                Seats = strSeats
            };

            //---get all the clients in dictionary---
            var query = (from c in clients
                         select c.Value).ToList();

            //---create the callback action---
            Action<ITicketCallBack> action =
                delegate(ITicketCallBack callback)
                {
                    //---callback to pass the seats booked 
                    // by a client to all other clients---        
                    callback.SeatStatus(_seatStatus.Seats);
                };

            //---for each connected client, invoke the callback--- 
            query.ForEach(action);
        }
    }
}

public class SeatStatus
{
    //---a string representing the seats booked by a client---
    public string Seats { get; set; }
}