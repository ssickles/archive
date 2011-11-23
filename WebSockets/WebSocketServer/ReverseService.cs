using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceModel.WebSockets;

namespace WebSocketServer
{
    class ReverseService: WebSocketsService
    {
        public override void OnMessage(string value)
        {
            string reverseString = string.Join("", value.Reverse());
            SendMessage(reverseString);
        }
    }
}
