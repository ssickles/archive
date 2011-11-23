using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceModel.WebSockets;

namespace WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebSocketsHost<ReverseService>();
            host.AddWebSocketsEndpoint("ws://void:9999/reverse");
            host.Open();
            Console.WriteLine("The reverse service is now hosted as a Web Sockets service.");
            Console.ReadLine();
        }
    }
}
