
using P2PChat;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Conversation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class test : ContentPage
    {
         Client client;


        public test()
        {
            InitializeComponent();

            try
            {
                client = new Client();


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
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }


        public void ReceiveMessage(Message M)
        {
            dialog.Text += M.From + ": " + M.Content + '\n';            
        }

        private void SendMessage(string message)
        {
            try
            {
                IPEndPoint ServerEndpoint = new IPEndPoint(IPAddress.Parse("13.81.63.14"), 50);
                Message M = new Message(client.LocalClientInfo.Name, client.LocalClientInfo.Name, message);
                client.SendMessageUDP(M, ServerEndpoint);
                // dialog.Text += ();
                dialog.Text += client.LocalClientInfo.Name + ": " + message + '\n';
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }

        
        private void Client_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                //dialog.Text += ("MessageReceived  =>  " + e.message + "(" + e.clientInfo.Name + " - " + e.EstablishedEP.ToString() + ")" + '\n');
                ReceiveMessage(e.message);
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }

        private void Client_OnClientConnection(object sender, System.Net.IPEndPoint e)
        {
            try
            {
                dialog.Text += ("OnClientConnection  =>  " + e.ToString() + '\n');
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }

        }

        private void Client_OnClientRemoved(object sender, Shared.ClientInfo e)
        {
            try
            {
                dialog.Text += ("OnClientRemoved  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")" + '\n');
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }

        private void Client_OnClientUpdated(object sender, Shared.ClientInfo e)
        {
            try
            {


                dialog.Text += ("OnClientUpdated  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")" + '\n');
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }

        private void Client_OnClientAdded(object sender, Shared.ClientInfo e)
        {
            try
            {
                dialog.Text += ("OnClientAdded  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")" + '\n');
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }

        private void Client_OnResultsUpdate(object sender, string e)
        {
            try
            {
                dialog.Text += ("OnResultsUpdate  =>  " + e + '\n');
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }

        private void Client_OnServerDisconnect(object sender, EventArgs e)
        {
            try
            {
                dialog.Text += ("OnServerDisconnect => Server Disconnected" + '\n');
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }

        private void Client_OnServerConnect(object sender, EventArgs e)
        {
            try
            {
                dialog.Text += ("OnServerConnect  =>  Server connected" + '\n');
            }
            catch (Exception ex)
            {
                dialog.Text += ex.Message;
            }
        }
    }
}