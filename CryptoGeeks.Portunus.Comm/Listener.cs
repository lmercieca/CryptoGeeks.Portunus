using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ProtoBuf;
using System.IO;
using CryptoGeeks.Portunus.Comm;

using Interceptor;
using CruptoGeeks.Portunus.Comm;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    public class TcpListenerDerivedClass : TcpListener
    {
        public TcpListenerDerivedClass(IPAddress address, int port) : base(address, port) { SetReUseAddress(); }
        public void SetReUseAddress()
        {
            Socket s = this.Server;
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
        }
    }

    public class TcpClientDerivedClass : TcpClient
    {
        string localEndpoint;
        int localPort;
        bool bindPort = false;

        public TcpClientDerivedClass(string address, int port, string localIp, bool bind) : base(address, port)
        {
            this.localEndpoint = localIp;
            this.localPort = port;
            this.bindPort = bind;
            SetReUseAddress();
        }
        public void SetReUseAddress()
        {
            Socket s = this.Client;
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

            if (bindPort)
            {
                s.Bind(new IPEndPoint(IPAddress.Parse(localEndpoint), localPort));
            }
        }
    }

    public class Listener
    {
        public delegate void OnNewConnectionHandler(object source, Payload payload, String message, IPEndPoint remoteEndpoint, IPEndPoint serverEndPoint);
        public event OnNewConnectionHandler OnNewConnection;

        public delegate void OnHandlePayloadHandler(Payload payload, bool isServerPayload);
        public event OnHandlePayloadHandler OnHandlePayload;

        public bool IsServerListener { get; set; }

        public int Port { get; set; }
        public NetworkStream ListenerStream = null;
        TcpClient Client = null;

        public void OnNewConnectionProxy(object source, Payload payload, String message, IPEndPoint remoteEndpoint, IPEndPoint serverEndPoint)
        {
            if (OnNewConnection != null)
                OnNewConnection(source, payload, message, remoteEndpoint, serverEndPoint);
        }

        public delegate void OnNewMessageHandler(object source, Payload payload, String message);
        public event OnNewMessageHandler OnNewMessage;

        public void OnNewMessageProxy(object source, Payload payload, String message)
        {
            if (OnNewMessage != null)
                OnNewMessage(source, payload, message);
        }

        public TcpListenerDerivedClass Server = null;
        public Listener(int port, bool isServerListener = true)
        {
            this.IsConnected = false;
            this.Port = port;
            Server = new TcpListenerDerivedClass(IPAddress.Any, port);
            this.IsServerListener = isServerListener;
            LoggerHelper.AddLog("Initiating on " + IPAddress.Any + ":" + port);
            Server.Start();
            LoggerHelper.AddLog("Listenening on " + IPAddress.Any + ":" + port);
        }

        public bool IsConnected { get; set; }

        public void StartListening()
        {
            try
            {

                Task tConn = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        OnNewMessageProxy(this, null, "Waiting for a connection...");
                        LoggerHelper.AddLog("Waiting for a connection...");

                        Client = Server.AcceptTcpClient();
                        this.IsConnected = true;
                        OnNewMessageProxy(this, null, "Connected!");
                        LoggerHelper.AddLog("Connected!");

                        Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                        t.Start(Client);
                    }
                });

            }
            catch (SocketException e)
            {
                OnNewMessageProxy(this, null, "SocketException: " + e);
                Server.Stop();
            }
        }


        public void Stop()
        {
            if (Server != null)
            {
                Server.Stop();
            }
        }

        public void TransmitData(Payload payload)
        {
            if (ListenerStream == null)
            {
                if (Client == null)
                {
                    var thread = new Thread(() =>
                    {
                        StartListening();
                        Thread.Sleep(1000);
                        ListenerStream = Client.GetStream();

                        Helper.SendPayload(ListenerStream, payload);
                    });
                    thread.Start();
                }
                else
                {
                    ListenerStream = Client.GetStream();

                    Helper.SendPayload(ListenerStream, payload);
                }

            }
            else
            {
                Helper.SendPayload(ListenerStream, payload);
            }
        }

        public void HandleDeivce(Object obj)
        {
            Client = (TcpClient)obj;
            ListenerStream = Client.GetStream();

            try
            {
                MemoryStream memStream = Helper.ReceiveStream(ListenerStream);
                MemoryStream cleanedStream = Helper.CleanIncomingStream(memStream);

                StreamReader reader = new StreamReader(cleanedStream);
                string text = reader.ReadToEnd();
                reader.Close();

                Payload payload = JsonConvert.DeserializeObject<Payload>(text);

                OnNewMessageProxy(this, payload, "Received: " + Helper.PrintPayload(payload));
                LoggerHelper.AddLog("Received: " + Helper.PrintPayload(payload));

                OnHandlePayload(payload, this.IsServerListener);
                StartListening();
            }
            catch (Exception e)
            {
                LoggerHelper.AddLog("Exception:" + e.ToString());
                OnNewMessageProxy(this, null, "Exception:" + e.ToString());

                Client.Close();
            }
        }
    }
}
