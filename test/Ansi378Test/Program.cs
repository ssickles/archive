using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ansi378Test.BiometricService;

namespace Ansi378Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BiometricServiceClient client = new BiometricServiceClient();
            byte[] temp = client.GetAnsi378Template(new Guid("95c7fff6-bd43-4557-9da9-2298d3db137e"), "L2");
            Console.WriteLine(temp.ToString());
            Console.ReadLine();
        }
    }
}
