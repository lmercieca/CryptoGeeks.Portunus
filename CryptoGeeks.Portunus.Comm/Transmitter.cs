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

        public delegate void OnNewMessageHandler( object source, Payload payload, String message);
        public event OnNewMessageHandler OnNewMessage;

        public void OnNewMessageProxy(object source, Payload payload, String message)
        {
            if (OnNewMessage != null)
                OnNewMessage(source, payload, message);
        }

        public void Connect(String server, int port, Payload message, bool bind)
        {
            try
            {

                TcpClientDerivedClass client = new TcpClientDerivedClass(server, port, Helper.GetLocalMachineIp(), bind);
                
                
                NetworkStream stream = client.GetStream();

                int bytesread = Helper.SendPayload(stream, message);
                OnNewMessageProxy(this, message, "Sent: {0} " +  Helper.PrintPayload(message));
                LoggerHelper.AddLog("Sent: {0} " + Helper.PrintPayload(message));

                // Bytes Array to receive Server Response.


                try
                {
                    MemoryStream memStream = Helper.ReceiveStream(stream);
                    MemoryStream cleanedStream = Helper.CleanIncomingStream(memStream);

                    StreamReader reader = new StreamReader(cleanedStream);
                    string text = reader.ReadToEnd();
                    reader.Close();

                    Payload payload = JsonConvert.DeserializeObject<Payload>(text);

                    OnNewMessageProxy(this, payload, "Received: {0} " + Helper.PrintPayload(payload));
                    LoggerHelper.AddLog("Received: {0} " + Helper.PrintPayload(payload));
                    Thread.Sleep(2000);

                }
                catch (Exception e)
                {
                    LoggerHelper.AddLog("Exception: " + e.ToString());
                    OnNewMessageProxy(this, null,  "Exception: " + e.ToString());                  

                    client.Close();
                }

             
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                LoggerHelper.AddLog("Exception: " + e.ToString());
                OnNewMessageProxy(this, null, "Exception: " + e.ToString());                

            }
        }
    }
}
