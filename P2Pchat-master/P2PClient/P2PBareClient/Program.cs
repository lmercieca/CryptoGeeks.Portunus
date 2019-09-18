using P2PChat;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PBareClient
{
    class Program
    {

        static Client client = new Client();

        static List<ChatWindow> ChatWindows = new List<ChatWindow>();

        static void Main(string[] args)
        {

            client.OnServerConnect += Client_OnServerConnect;
            client.OnServerDisconnect += Client_OnServerDisconnect;
            client.OnResultsUpdate += Client_OnResultsUpdate;
            client.OnClientAdded += Client_OnClientAdded;
            client.OnClientUpdated += Client_OnClientUpdated;
            client.OnClientRemoved += Client_OnClientRemoved;
            client.OnClientConnection += Client_OnClientConnection;
            client.OnMessageReceived += Client_OnMessageReceived;


            client.ConnectOrDisconnect();

            SendMessage("Hello");

            while(true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static void ReceiveMessage(Message M)
        {
            Console.WriteLine("Received => " + M.From + ": " + M.Content + "\"");
        }

        private static void SendMessage(string message)
        {
            IPEndPoint ServerEndpoint = new IPEndPoint(IPAddress.Parse("13.81.63.14"), 50);
            Message M = new Message(client.LocalClientInfo.Name, client.LocalClientInfo.Name, message);
            client.SendMessageUDP(M, ServerEndpoint);
            Console.WriteLine(client.LocalClientInfo.Name + ": " + message);
        }


        private static void Client_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("MessageReceived  =>  " + e.message + "(" + e.clientInfo.Name + " - " + e.EstablishedEP.ToString() + ")");
        }

        private static void Client_OnClientConnection(object sender, System.Net.IPEndPoint e)
        {
            Console.WriteLine("OnClientConnection  =>  " + e.ToString());

        }

        private static void Client_OnClientRemoved(object sender, Shared.ClientInfo e)
        {
            Console.WriteLine("OnClientRemoved  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")");
        }

        private static void Client_OnClientUpdated(object sender, Shared.ClientInfo e)
        {
            Console.WriteLine("OnClientUpdated  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")");
        }

        private static void Client_OnClientAdded(object sender, Shared.ClientInfo e)
        {
            Console.WriteLine("OnClientAdded  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")");
        }

        private static void Client_OnResultsUpdate(object sender, string e)
        {
            Console.WriteLine("OnResultsUpdate  =>  " + e);
        }

        private static void Client_OnServerDisconnect(object sender, EventArgs e)
        {
            Console.WriteLine("OnServerDisconnect => Server Disconnected");
        }

        private static void Client_OnServerConnect(object sender, EventArgs e)
        {
            Console.WriteLine("OnServerConnect  =>  Server connected");
        }
    }
}
