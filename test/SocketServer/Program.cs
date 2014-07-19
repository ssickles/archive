using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(8000);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            byte[] input = new byte[client.ReceiveBufferSize];
            stream.Read(input, 0, client.ReceiveBufferSize);
            string inputString = Encoding.ASCII.GetString(input);
            inputString = string.Format("echo: {0}", inputString);
            byte[] output = Encoding.ASCII.GetBytes(inputString);
            stream.Write(output, 0, output.Length);
            Console.ReadLine();
        }
    }
}
