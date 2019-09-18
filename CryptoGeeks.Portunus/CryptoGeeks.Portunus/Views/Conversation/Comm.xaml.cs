using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Comm;
using CryptoGeeks.Portunus.CommunicationFramework;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Models;
using CryptoGeeks.Portunus.ViewModels;
using CryptoGeeks.Portunus.Views.AddKey;
using CryptoGeeks.Portunus.Views.ExportImport;
using CryptoGeeks.Portunus.Views.Recovery;
using P2PChat;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;


namespace CryptoGeeks.Portunus.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Comm : ContentPage
    {
        Client client;
        ObservableCollection<ClientInfo> contacts = new ObservableCollection<ClientInfo>();


        public Comm()
        {
            InitializeComponent();

            try
            {
                client = new Client();

                ContactsListView.ItemsSource = contacts;

                client.OnServerConnect += Client_OnServerConnect;
                client.OnServerDisconnect += Client_OnServerDisconnect;
                client.OnResultsUpdate += Client_OnResultsUpdate;
                client.OnClientAdded += Client_OnClientAdded;
                client.OnClientUpdated += Client_OnClientUpdated;
                client.OnClientRemoved += Client_OnClientRemoved;
                client.OnClientConnection += Client_OnClientConnection;
                client.OnMessageReceived += Client_OnMessageReceived;


                client.ConnectOrDisconnect();

                IPEndPoint ServerEndpoint = new IPEndPoint(IPAddress.Parse("13.81.63.14"), 50);
                SendMessage("Sending Ehlo", ServerEndpoint);
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }

        }

        private void BtnSend_Clicked(object sender, EventArgs e)
        {
            ClientInfo ci = (ClientInfo)ContactsListView.SelectedItem;

            IPEndPoint endPoint = client.ConnectToClient(ci);

            SendMessage(txtMessage.Text, endPoint);
        }

        private void BtnSendBeat_Clicked(object sender, EventArgs e)
        {
            IPEndPoint ServerEndpoint = new IPEndPoint(IPAddress.Parse("13.81.63.14"), 50);

            SendMessage("Beat", ServerEndpoint);
        }


        public void ReceiveMessage(Message M)
        {
            txtIncoming.Text += M.From + ": " + M.Content + '\n';
        }

        private void SendMessage(string message, IPEndPoint endpoint)
        {
            try
            {
                Message M = new Message(client.LocalClientInfo.Name, client.LocalClientInfo.Name, message);
                client.SendMessageUDP(M, endpoint);
                // txtIncoming.Text += ();
                txtIncoming.Text += client.LocalClientInfo.Name + ": " + message + '\n';
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }


        private void Client_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                //txtIncoming.Text += ("MessageReceived  =>  " + e.message + "(" + e.clientInfo.Name + " - " + e.EstablishedEP.ToString() + ")" + '\n');
                ReceiveMessage(e.message);
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }

        private void Client_OnClientConnection(object sender, System.Net.IPEndPoint e)
        {
            try
            {
                txtIncoming.Text += ("OnClientConnection  =>  " + e.ToString() + '\n');
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }

        }

        private void Client_OnClientRemoved(object sender, Shared.ClientInfo e)
        {
            try
            {
                txtIncoming.Text += ("OnClientRemoved  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")" + '\n');

                contacts.Clear();

                foreach (ClientInfo ci in contacts)
                {
                    if (ci.ID == e.ID)
                        contacts.Remove(ci);
                }
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }

        private void Client_OnClientUpdated(object sender, Shared.ClientInfo e)
        {
            try
            {


                txtIncoming.Text += ("OnClientUpdated  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")" + '\n');

                
                for (int nCnt=0; nCnt<contacts.Count; nCnt++)
                {
                    if (contacts[nCnt].ID == e.ID)
                        contacts[nCnt] = e;
                }


            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }

        private void Client_OnClientAdded(object sender, Shared.ClientInfo e)
        {
            try
            {
                txtIncoming.Text += ("OnClientAdded  =>  " + e.Name + "(" + e.InternalEndpoint.ToString() + " - PnpEnabled: " + e.UPnPEnabled + ")" + '\n');

                contacts.Add(e);
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }

        private void Client_OnResultsUpdate(object sender, string e)
        {
            try
            {
                txtIncoming.Text += ("OnResultsUpdate  =>  " + e + '\n');
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }

        private void Client_OnServerDisconnect(object sender, EventArgs e)
        {
            try
            {
                txtIncoming.Text += ("OnServerDisconnect => Server Disconnected" + '\n');
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }

        private void Client_OnServerConnect(object sender, EventArgs e)
        {
            try
            {
                txtIncoming.Text += ("OnServerConnect  =>  Server connected" + '\n');
            }
            catch (Exception ex)
            {
                txtIncoming.Text += ex.Message;
            }
        }
    }
}