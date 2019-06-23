using Newtonsoft.Json;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.ClientTestTools
{
    class Program
    {

        public async static void SendData(string address, int port, string message, string displayName)
        {
            var client = new UdpSocketClient();

            Message msg = new Message();
            msg.Data = message;
            msg.DisplayName = displayName;

            string serializedMsg = JsonConvert.SerializeObject(msg);

            byte[] payLoad = ASCIIEncoding.ASCII.GetBytes(serializedMsg);
            await client.SendToAsync(payLoad, address, port);
        }


        static void Main(string[] args)
        {

            var address = "40.114.212.129";
            var port = 11000;

            SendData(address, port, "hello", "displayName");

            while (true)
            {

            }
        }

    }
}