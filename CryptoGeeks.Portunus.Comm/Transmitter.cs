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

        public void Connect(String server, int port, Payload message)
        {
            try
            {

                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();

                int bytesread = Helper.SendPayload(stream, message);
                OnNewMessageProxy(this, message, "Sent: {0} " +  Helper.PrintPayload(message));
                
                // Bytes Array to receive Server Response.
                
                
                try
                {
                    MemoryStream memStream = Helper.ReceiveStream(stream);
                    MemoryStream cleanedStream = Helper.CleanIncomingStream(memStream);
                    Payload payload = Serializer.Deserialize<Payload>(cleanedStream);

                    OnNewMessageProxy(this, payload, "Sent: {0} " + Helper.PrintPayload(payload));                    
                    Thread.Sleep(2000);

                }
                catch (Exception e)
                {
                    OnNewMessageProxy(this, null,  "Exception: " + e.ToString());                    
                    client.Close();
                }

             
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                OnNewMessageProxy(this, null, "Exception: " + e.ToString());
            }
            Console.Read();
        }
    }
}
