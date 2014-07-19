using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMS_Web_Service_Test.OfsProcessingReference;

namespace JMS_Web_Service_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            OfsProcessingClient client = new OfsProcessingClient();
            //bool result = client.TransactionRequest("ConnectionFactory", "queue/t24OFSQueue", "queue/t24OFSReplyQueue", "FUNDS.TRANSFER,AUTH.HP/A/PROCESS,HEADTELLER01/123456,FT09007VKZPQ//1,");
            bool result = client.TransactionRequest("ConnectionFactory", "queue/t24OFSQueue", "queue/t24OFSReplyQueue", "FUNDS.TRANSFER,AUTH.HP/A/PROCESS,HEADTELLER01/123456,FT091876P0QV//1,");
            //bool result = client.TransactionRequest("ConnectionFactory", "queue/t24OFSQueue", "queue/t24OFSReplyQueue", "ENQUIRY.SELECT,,AUTHOR/123456,CURRENCY-LIST");
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
