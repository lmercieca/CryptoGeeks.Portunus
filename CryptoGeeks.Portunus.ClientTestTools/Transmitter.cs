using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Server
{
    public class Transmitter
    {
        public async static void SendData(string address, int port, string message)
        {
            var client = new UdpSocketClient();
            byte[] payLoad = ASCIIEncoding.ASCII.GetBytes(message);
            await client.SendToAsync(payLoad, address, port);
        }
    }
}
