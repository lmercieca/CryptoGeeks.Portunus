using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    public class Transmitter
    {

        public delegate void OnNewMessageHandler(object source, Payload payload, String message);
        public event OnNewMessageHandler OnNewMessage;

        public delegate void OnNewPayloadHandler(object source, Payload payload);
        public event OnNewPayloadHandler OnNewPayload;

        TcpClientDerivedClass client;

        public void OnNewPayloadProxy(object source, Payload payload)
        {
            if (OnNewPayload != null)
                OnNewPayload(source, payload);
        }

        public void OnNewMessageProxy(object source, Payload payload, String message)
        {
            if (OnNewMessage != null)
                OnNewMessage(source, payload, message);
        }

        public void SendData(Payload payload)
        {
            if (client != null)
            {

                NetworkStream stream = client.GetStream();
                Helper.SendPayload(stream, payload);
            }

        }

        public int Port { get; set; }

        public void Close()
        {
            NetworkStream stream = client.GetStream();

            stream.Close();
            client.Close();
        }
        public void Connect(String server, int port, Payload message, bool bind)
        {
            try
            {
                this.Port = port;
                client = new TcpClientDerivedClass(server, port, Helper.GetLocalMachineIp(), bind);

                NetworkStream stream = client.GetStream();

                int bytesread = Helper.SendPayload(stream, message);
                // OnNewMessageProxy(this, message, "Sent: {0} " +  Helper.PrintPayload(message));
                LoggerHelper.AddLog("Sent: {0} " + Helper.PrintPayload(message));

                // Bytes Array to receive Server Response.
                var thread = new Thread(() =>
                {
                    MemoryStream memStream = Helper.ReceiveStream(stream);
                    MemoryStream cleanedStream = Helper.CleanIncomingStream(memStream);

                    StreamReader reader = new StreamReader(cleanedStream);
                    string text = reader.ReadToEnd();
                    reader.Close();

                    Payload payload = JsonConvert.DeserializeObject<Payload>(text);
                    OnNewPayloadProxy(this, payload);
                    //OnNewMessageProxy(this, payload, "Received: {0} " + Helper.PrintPayload(payload));
                    LoggerHelper.AddLog("Received: {0} " + Helper.PrintPayload(payload));
                    Thread.Sleep(2000);

                });

                thread.Start();

            }
            catch (Exception e)
            {
                LoggerHelper.AddLog("Exception: " + e.ToString());
                OnNewMessageProxy(this, null, "Exception: " + e.ToString());

            }
        }
    }
}
