using CryptoGeeks.Portunus.Comm;
using CryptoGeeks.Portunus.CommunicationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CruptoGeeks.Server.Host
{
    public class MainChannel
    {
        public delegate void OnNewClientHandler(string remoteIp, int remotePort, string serverIp, int serverPort, int userId);
        public event OnNewClientHandler OnNewClient;



        PayloadProcessor payloadProcessor = new PayloadProcessor();
        Listener listener = null;

        List<Conduit> clientsChannels = new List<Conduit>();
        Dictionary<int, Listener> clients = new Dictionary<int, Listener>();
        Listener clientListener;

        public void CloseConnections()
        {
            foreach (Conduit c in clientsChannels)
            {
                c.Close();
            }

            if (listener != null)
                listener.Stop();


        }

        public MainChannel()
        {


        }
        public void StartListening(int port)
        {
            listener = new Listener(port);
            listener.OnNewMessage += Listener_OnNewMessage;
            listener.OnNewConnection += Listener_OnNewConnection;
            listener.OnHandlePayload += Listener_OnHandlePayload;
            listener.StartListening();
        }

        private void Listener_OnHandlePayload(Payload payload, bool isServerListener)
        {
            HandlePayload(ref payload);
        }

        private void Listener_OnNewConnection(object source, Payload payload, string message, IPEndPoint remoteEndpoint, IPEndPoint serverEndPoint)
        {
            //clients.Add(payload.OwnerUserId);

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

                    if (!clients.ContainsKey(payload.OwnerUserId))
                    {
                        int port = PortService.GetInstance().GetNextPort();

                        clientListener = new Listener(port);
                        clientListener.OnNewMessage += Listener_OnNewMessage;
                        clientListener.OnNewConnection += Listener_OnNewConnection;
                        clientListener.OnHandlePayload += Listener_OnHandlePayload;

                        clients.Add(payload.OwnerUserId, clientListener);
                        clientListener.StartListening();

                        Payload p = new Payload(MessageType.RequestForChannel, MessageSource.Server, MessageState.Request,
                        DataType.ContactRequest, payload.OwnerUserId, port.ToString());

                        OnNewClient(payload.FromIp, -1, "127.0.0.1", -1, payload.OwnerUserId);

                        listener.TransmitData(p);
                    }
                    else
                    {
                        payload.MessageState = MessageState.Response;
                        clients[payload.OwnerUserId].TransmitData(payload);
                    }
                    break;

                case MessageType.RequestForClose:
                    string id = payload.AdditionalData;

                    Conduit cond = clientsChannels.Where(x => x.ID == id).FirstOrDefault();

                    if (cond != null)
                    {
                        cond.Close();
                        clientsChannels.Remove(cond);
                    }
                    break;

                case MessageType.RequestForChannel:

                    if (clients.ContainsKey(payload.OwnerUserId) && clients.ContainsKey(int.Parse(payload.AdditionalData)))
                    {
                        int primaryPort = PortService.GetInstance().GetNextPort();
                        int secondaryPort = PortService.GetInstance().GetNextPort();

                        Listener primary = new Listener(primaryPort);
                        Listener secondary = new Listener(secondaryPort);


                        primary.StartListening();
                        secondary.StartListening();


                        Conduit conduit = new Conduit(primary, secondary, payload.OwnerUserId, int.Parse(payload.AdditionalData));
                        conduit.InitiateChannel();


                        Payload p = new Payload(MessageType.RequestForChannel, MessageSource.Server, MessageState.Response,
                        DataType.ContactRequest, int.Parse(payload.AdditionalData), secondaryPort.ToString(), conduit.ID);

                        Listener main = clients[payload.OwnerUserId];
                        main.TransmitData(p);


                        var thread = new Thread(() =>
                        {
                            conduit.StartChannel();
                        });

                        p = new Payload(MessageType.RequestForChannel, MessageSource.Server, MessageState.Response,
                        DataType.ContactRequest, payload.OwnerUserId, primaryPort.ToString(), conduit.ID);

                        Listener sec = clients[int.Parse(payload.AdditionalData)];
                        sec.TransmitData(p);


                        clientsChannels.Add(conduit);

                    }
                    break;

                case MessageType.ClosePeerConnection:
                    string Id = payload.PayloadData;
                    Conduit c = clientsChannels.Where(x => x.ID == Id).FirstOrDefault();

                    if (c != null)
                    {
                        c.Close();
                        clientsChannels.Remove(c);
                    }

                    break;


                case MessageType.CloseServerConnection:

                    int user = payload.OwnerUserId;

                    List<Conduit> conduits = clientsChannels.Where(x => x.PrimaryUserId == user || x.SecondaryUserId == user).ToList();



                    foreach (Conduit conduit in conduits)
                    {

                        Payload closeChannelPayload = new Payload(MessageType.ClosePeerConnection, MessageSource.Server, MessageState.Request,
                  DataType.ContactRequest, user, conduit.ID);

                        

                            Listener sec = clients[conduit.SecondaryUserId];
                        sec.TransmitData(closeChannelPayload);


                        conduit.Close();
                        clientsChannels.Remove(conduit);
                    }

                    this.clients.Remove(user);

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
