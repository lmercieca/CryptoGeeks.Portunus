using CryptoGeeks.Portunus.CommunicationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CruptoGeeks.Portunus.Comm
{
    public class ClientConnection
    {
        public int UserID { get; set; }
        public Listener Listener;
        public int Port { get; set; }
        public Thread CurrentThread = null;

        public ClientConnection(int port)
        {
            this.Port = port;
            Listener = new Listener(port);
            Listener.OnNewMessage += Listener_OnNewMessage;
            Listener.OnNewConnection += Listener_OnNewConnection;

            CurrentThread = new Thread(() => Listener.StartListening());
            CurrentThread.Start();
            
        }

        private void Listener_OnNewConnection(object source, Payload payload, string message, System.Net.IPEndPoint remoteEndpoint, System.Net.IPEndPoint serverEndPoint)
        {
            
        }

        private void Listener_OnNewMessage(object source, Payload payload, string message)
        {
            if (payload != null)
            {
                payload.PayloadData = "Rec " + payload.PayloadData;
                Listener.TransmitData(payload);
            }

        }

        public void Close()
        {
            if (Listener != null)
                Listener.Stop();

            if (CurrentThread != null)
                CurrentThread.Abort();
        }
    }

    public class ClientConnectionManager
    {
        private static ClientConnectionManager instance = null;

        public static ClientConnectionManager GetInstance()
        {
            if (instance == null)
                instance = new ClientConnectionManager();

            return instance;
        }

        private ClientConnectionManager()
        {
            ServerConnections = new Dictionary<int, ClientConnection>();
        }
        public Dictionary<int, ClientConnection> ServerConnections = new Dictionary<int, ClientConnection>();

        public ClientConnection AddServerConnection(int clientId, int port)
        {
            ClientConnection conn;

            if (!ServerConnections.ContainsKey(clientId))
            {
                conn = new ClientConnection(port);
            }
            else
            {
                conn = ServerConnections[clientId];
                if (conn.Port != port)
                {
                    conn.Close();
                    conn = new ClientConnection(port);
                }
            }

            return conn;
        }
    }
}
