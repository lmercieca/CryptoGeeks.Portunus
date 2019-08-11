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
        public void Connect(String server, int port, Payload message)
        {
            try
            {

                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();

                int bytesread = Helper.SendPayload(stream, message);                               
                Console.WriteLine("Sent: {0}", Helper.PrintPayload(message));
                // Bytes Array to receive Server Response.
                
                
                try
                {
                    MemoryStream memStream = Helper.ReceiveStream(stream);
                    MemoryStream cleanedStream = Helper.CleanIncomingStream(memStream);
                    Payload payload = Serializer.Deserialize<Payload>(cleanedStream);

                    Console.WriteLine("Received: {0}", Helper.PrintPayload(payload));
                    Thread.Sleep(2000);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.ToString());
                    client.Close();
                }

             
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            Console.Read();
        }
    }
}
