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
        public delegate void OnNewClientHandler(string remoteIp, int remotePort, string serverIp, int serverPort, int userId);
        public event OnNewClientHandler OnNewClient;


        TcpListener listenerFromServer = null;
        TcpClient client = null;

        PayloadProcessor payloadProcessor = new PayloadProcessor();
        Listener listener = null;

        List<Conduit> clientsChannels = new List<Conduit>();
        List<int> clients = new List<int>();


        public MainChannel()
        {


        }
        public void StartListening(int port)
        {
            listener = new Listener(port);
            listener.OnNewMessage += Listener_OnNewMessage;
            listener.OnNewConnection += Listener_OnNewConnection;
            listener.StartListening();
        }

        private void Listener_OnNewConnection(object source, Payload payload, string message, IPEndPoint remoteEndpoint, IPEndPoint serverEndPoint)
        {
            clients.Add(payload.OwnerUserId);


            if (OnNewClient != null)
                OnNewClient(remoteEndpoint.Address.ToString(), remoteEndpoint.Port, serverEndPoint.Address.ToString(), serverEndPoint.Port, payload.OwnerUserId);
        }



        private void Listener_OnNewMessage(object source, Payload payload, string message)
        {
            if (payload != null)
            {
                Console.WriteLine(Helper.PrintPayload(payload));
                Console.WriteLine(message);

                HandlePayload(ref payload);
            }


        }


        public void HandlePayload(ref Payload payload)
        {
            switch (payload.MessageType)
            {
                case MessageType.Ping:
                    MarkPing(payload);
                    break;

                case MessageType.RequestForOpen:

                    break;

                case MessageType.RequestForChannel:

                    TcpListenerDerivedClass primary = new TcpListenerDerivedClass(IPAddress.Any,
                        PortService.GetInstance().GetNextPort());
                    TcpListenerDerivedClass secondary = new TcpListenerDerivedClass(IPAddress.Any,
                        PortService.GetInstance().GetNextPort());

                    Conduit conduit = new Conduit(primary, secondary);
                    conduit.InitiateChannel();

                    clientsChannels.Add(conduit);

                    break;
            }
        }

        public void MarkPing(Payload payload)
        {
            PortunusEntity db = new PortunusEntity();

            Ping ping = new Ping();
            ping.User_Fk = payload.OwnerUserId;
            ping.Time = DateTime.Now;
            ping.SourceIp = payload.FromIp;

            db.Pings.Add(ping);


        }

    }
}
