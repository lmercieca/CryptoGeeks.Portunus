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
using Newtonsoft.Json;

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
        public delegate void OnNewMessageHandler(object source, Payload payload, String message);
        public event OnNewMessageHandler OnNewMessage;

        public void OnNewMessageProxy(object source, Payload payload, String message)
        {
            if (OnNewMessage != null)
                OnNewMessage(source, payload, message);
        }


        public TcpListenerDerivedClass Server = null;
        public Listener(int port)
        {
            Server = new TcpListenerDerivedClass(IPAddress.Any, port);

            LoggerHelper.AddLog("Initiating on " + IPAddress.Any + ":" + port);
            Server.Start();
            LoggerHelper.AddLog("Listenening on " + IPAddress.Any + ":" + port);
        }

        public void StartListening()
        {
            try
            {
                while (true)
                {
                    OnNewMessageProxy(this, null, "Waiting for a connection...");
                    LoggerHelper.AddLog("Waiting for a connection...");

                    TcpClient client = Server.AcceptTcpClient();
                    OnNewMessageProxy(this, null, "Connected!");
                    LoggerHelper.AddLog("Connected!");

                    Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                OnNewMessageProxy(this, null, "SocketException: " + e);
                Server.Stop();
            }
        }
        public void HandleDeivce(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();


            try
            {
                MemoryStream memStream = Helper.ReceiveStream(stream);
                MemoryStream cleanedStream = Helper.CleanIncomingStream(memStream);

                StreamReader reader = new StreamReader(cleanedStream);
                string text = reader.ReadToEnd();
                reader.Close();



                Payload payload = JsonConvert.DeserializeObject<Payload>(text);
                //Payload payload = Serializer.Deserialize<Payload>(cleanedStream);

                OnNewMessageProxy(this, payload, "Received: " + Helper.PrintPayload(payload));
                LoggerHelper.AddLog("Received: " + Helper.PrintPayload(payload));

                PayloadProcessor proc = new PayloadProcessor();
                proc.HandlePayload(ref payload);

                payload.PayloadData = "Ack data: " + payload.PayloadData;
                int bytesread = Helper.SendPayload(stream, payload);

                OnNewMessageProxy(this, payload, "Sent: " + Helper.PrintPayload(payload));
                LoggerHelper.AddLog("Sent: " + Helper.PrintPayload(payload));

                Helper.SendPayload(stream, payload);


            }
            catch (Exception e)
            {
                LoggerHelper.AddLog("Exception:" + e.ToString());
                OnNewMessageProxy(this, null, "Exception:" + e.ToString());

                client.Close();
            }
        }
    }
}
