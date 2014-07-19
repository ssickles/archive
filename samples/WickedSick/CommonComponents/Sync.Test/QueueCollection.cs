using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CommonComponents.Sync.Test
{
    public class QueueCollection: ISync
    {
        private string _name = string.Empty;
        private Dictionary<string, object> _queues = new Dictionary<string, object>();

        public QueueCollection()
        {

        }

        #region ISync Members

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public SyncResult Run()
        {
            try
            {
                List<object> _listOfQueues;
                //_listOfQueues = BPMQueueClient.GetAllQueues(string ApplicationName, string UserName);
                //compare both lists using Queue.Status
                //if new Queues added on server, add to _queues Dictionary
                //if Queues removed on server, remove from _queues Dictionary
                //if modified on the server, update the corresponding Queue
                //if modified, added, or removed on the client, send updates to server
            }
            catch (Exception ex)
            {
                return SyncResult.Failure;
            }

            //for testing purposes, simulate some work being done
            Thread.Sleep(10000);
            //for testing purposes, use a random number to dictate whether the sync succeded
            Random rand = new Random();
            if (rand.Next(2) == 1)
            {
                Console.WriteLine("Failure");
            }
            else
            {
                Console.WriteLine("Success");
            }

            return SyncResult.Success;
        }

        public Dictionary<string, object> Queues
        {
            get { return _queues; }
        }

        #endregion
    }
}
