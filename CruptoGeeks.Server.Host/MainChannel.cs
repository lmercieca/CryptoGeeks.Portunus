using CryptoGeeks.Portunus.Comm;
using CryptoGeeks.Portunus.CommunicationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CruptoGeeks.Server.Host
{
    public class MainChannel
    {
        TcpListener listenerFromServer = null;
        TcpClient client = null;

        PayloadProcessor payloadProcessor = new PayloadProcessor();
        Listener listener = null;
        
        public MainChannel(int port)
        {
            listener = new Listener(port);
            listener.OnNewMessage += Listener_OnNewMessage;
            listener.StartListening();

        }

        private void Listener_OnNewMessage(object source, Payload payload, string message)
        {
            if (payload != null)
            {
                Console.WriteLine(Helper.PrintPayload(payload));
                Console.WriteLine(message);

                payloadProcessor.HandlePayload(ref payload);

                
                
                
            }
        }
    }
}
