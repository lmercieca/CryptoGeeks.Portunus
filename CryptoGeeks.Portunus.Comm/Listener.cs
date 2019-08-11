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

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    public class Listener
    {
        TcpListener server = null;
        public Listener(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();
            StartListening();
        }

        public void StartListening()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
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
                Payload payload = Serializer.Deserialize<Payload>(cleanedStream);
                Console.WriteLine("Received: ", Helper.PrintPayload(payload));

                PayloadProcessor proc = new PayloadProcessor();
                proc.HandlePayload(ref payload);

                payload.PayloadData = "Ack data: " + payload.PayloadData;
                int bytesread = Helper.SendPayload(stream, payload);

                Console.WriteLine("Sent: {0}", Helper.PrintPayload(payload));

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }

    

    }
}
