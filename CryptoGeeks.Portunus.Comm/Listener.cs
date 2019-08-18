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
    public class Listener
    {
        public delegate void OnNewMessageHandler(object source, Payload payload, String message);
        public event OnNewMessageHandler OnNewMessage;

        public void OnNewMessageProxy(object source, Payload payload, String message)
        {
            if (OnNewMessage != null)
                OnNewMessage(source, payload, message);
        }


        TcpListener server = null;
        public Listener(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            LoggerHelper.AddLog("Initiating on " + localAddr + ":" + port);
            server.Start();
            LoggerHelper.AddLog("Listenening on " + localAddr + ":" + port);
        }

        public void StartListening()
        {
            try
            {
                while (true)
                {
                    OnNewMessageProxy(this, null, "Waiting for a connection...");
                    LoggerHelper.AddLog("Waiting for a connection...");

                    TcpClient client = server.AcceptTcpClient();
                    OnNewMessageProxy(this, null, "Connected!");
                    LoggerHelper.AddLog("Connected!");

                    Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                OnNewMessageProxy(this, null, "SocketException: " +  e);
                server.Stop();
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

                

            }
            catch (Exception e)
            {
                LoggerHelper.AddLog("Exception:" + e.ToString());
                OnNewMessageProxy(this, null, "Exception:" +  e.ToString());
              
                client.Close();
            }
        }
    }
}
