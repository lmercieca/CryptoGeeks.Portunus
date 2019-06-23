
using Newtonsoft.Json;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Comm
{
    public class Receiver
    {

        UdpSocketReceiver receiver = new UdpSocketReceiver();

        public async void Listen(int listenPort)
        {

            receiver.MessageReceived += (sender, args) =>
            {
                
                //var from = String.Format("{0}:{1}", args.RemoteAddress, args.RemotePort);
                var data = Encoding.UTF8.GetString(args.ByteData, 0, args.ByteData.Length);

                QueueManager.Instance.PushToQueue(JsonConvert.DeserializeObject<Message>(data));

                Console.WriteLine(data);
            };



            // listen for udp traffic on listenPort
            await receiver.StartListeningAsync(listenPort);
        }

    }
}
