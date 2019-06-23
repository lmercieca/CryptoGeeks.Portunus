using Newtonsoft.Json;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Comm
{
    public class Transmitter
    {
        public async static void SendData(string address, int port, Message message)
        {
            var client = new UdpSocketClient();
            string json = JsonConvert.SerializeObject(message);
            byte[] payLoad = ASCIIEncoding.ASCII.GetBytes(json);
            await client.SendToAsync(payLoad, address, port);
        }
    }
}
