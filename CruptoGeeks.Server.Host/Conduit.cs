﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using CryptoGeeks.Portunus.CommunicationFramework;

namespace CruptoGeeks.Server.Host
{
    public class Conduit
    {
        public string ID { get; set; }

        public int PrimaryUserId { get; set; }
        public int SecondaryUserId { get; set; }

        Listener listenerFromClient;
        Listener listenerFromServer;

        TcpClient client = null;
        TcpClient server = null;

        

        public Conduit(Listener client, Listener server, int primaryUserId, int secondaryUserId)
        {
            this.ID = Guid.NewGuid().ToString();
            this.PrimaryUserId = primaryUserId;
            this.SecondaryUserId = secondaryUserId;

            listenerFromClient = client;
            listenerFromServer = server;
        }

        public void InitiateChannel()
        {
            Console.WriteLine("Interceptor Listening...");
            listenerFromClient.Server.Start();
            listenerFromServer.Server.Start();
        }

        public void StartChannel()
        {
            //---incoming client connected---
            client = listenerFromClient.Server.AcceptTcpClient();
            server = listenerFromServer.Server.AcceptTcpClient();

            //---get the incoming data through a network stream---
            NetworkStream nwStreamClient = client.GetStream();
            NetworkStream nwStreamServer = server.GetStream();


            byte[] bufferClient = new byte[client.ReceiveBufferSize];
            byte[] bufferServer = new byte[server.ReceiveBufferSize];

            //---read incoming stream---
            int bytesReadClient = nwStreamClient.Read(bufferClient, 0, client.ReceiveBufferSize);
            int bytesReadServer = nwStreamServer.Read(bufferServer, 0, server.ReceiveBufferSize);

            //---convert the data received into a string---
            string dataReceivedFromClient = Encoding.ASCII.GetString(bufferClient, 0, bytesReadClient) + " from client";
            string dataReceivedFromServer = Encoding.ASCII.GetString(bufferServer, 0, bytesReadServer) + " from server";

            Console.WriteLine("Received : " + dataReceivedFromClient);
            Console.WriteLine("Received : " + dataReceivedFromServer);

            //---write back the text to the client---
            Console.WriteLine("Sending back to server: " + dataReceivedFromClient);
            Console.WriteLine("Sending back to client: " + dataReceivedFromServer);

            nwStreamClient.Write(bufferServer, 0, bytesReadServer);
            nwStreamServer.Write(bufferClient, 0, bytesReadClient);

        }

        public void Close()
        {
            if (client != null)
            {
                client.Close();
                listenerFromClient.Stop();
            }

            if (client != null)
            {
                listenerFromServer.Stop();
                server.Close();
            }
        }
    }
}
