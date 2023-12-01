using BNMS;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new Server
            {
                Services = { BNMSStatusSrv.BindService(new BNMSStatusServer()) },
                Ports = { new ServerPort("localhost", 50051, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine($"MyTestGrpc Listen to Port: 555");
            Console.ReadLine();
        }
    }
}
