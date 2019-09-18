using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Comm;
using CryptoGeeks.Portunus.CommunicationFramework;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Models;
using CryptoGeeks.Portunus.ViewModels;
using CryptoGeeks.Portunus.Views.AddKey;
using CryptoGeeks.Portunus.Views.ExportImport;
using CryptoGeeks.Portunus.Views.Recovery;

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
    public partial class Conversation : ContentPage
    {
        Workflow workflow = new Workflow();
        public MultiSelectObservableCollection<ContactViewModel> Contacts;

        Dictionary<string, int> ContactReference = new Dictionary<string, int>();



        


        public Conversation()
        {
            InitializeComponent();


            workflow.OnNewMessage += Workflow_OnNewMessage;
            
   
            workflow.StartListener(11000);

            ItemListViewModel itemListViewModel = new ItemListViewModel();
            Task<string> userTask = itemListViewModel.RefreshData();
            userTask.Wait();
            Contacts = itemListViewModel.Contacts;

            workflow.OnNewMessage += Workflow_OnNewMessage;

            foreach (SelectableItem<ContactViewModel> cvm in itemListViewModel.Contacts)
            {
                ContactsListView.Items.Add(cvm.Data.DisplayName);
                ContactReference.Add(cvm.Data.DisplayName, cvm.Data.Id);
            }

            /*
            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                //Listener myserver = new Listener("192.168.***.***", 13000);
                Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, 1, "Hello Buddy");
                workflow.TransmitData("13.81.63.14", 11000, payload);
                System.Threading.Thread.Sleep(1000);

            });*/
        }




        private void Workflow_OnNewMessage(object source, Payload payload, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                txtIncoming.Text = txtIncoming.Text + Environment.NewLine + message;
            });
        }

        private void BtnSend_Clicked(object sender, EventArgs e)
        {
            PayloadProcessor proc = new PayloadProcessor();

            CancellationToken canTok = new CancellationToken();
            Task<List<UserStatusCompact>> userTask = proc.GetUsersConnection(canTok);
            userTask.Wait();

            int selectedId = ContactReference[ContactsListView.SelectedItem.ToString()];
            UserStatusCompact peer = (from x in userTask.Result where x.Id == selectedId select x).FirstOrDefault();

            if (peer != null)
            {

                CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
                int userId = int.Parse(secureStorage.GetFromSecureStorage(Constants.UserId));

                Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, userId, txtMessage.Text);
                payload.FromIp = Helper.GetPublicMachineIp();

                ipFrom.Text = payload.FromIp;
                ipTo.Text = peer.SourceIp;

                workflow.TransmitData(peer.SourceIp, 11000, payload, true);

                Thread.Sleep(100);
            }
        }

        private void BtnSendBeat_Clicked(object sender, EventArgs e)
        {
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            int userId = int.Parse(secureStorage.GetFromSecureStorage(Constants.UserId));

            Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, userId, "Hello Buddy from user " + userId);
            payload.FromIp = Helper.GetPublicMachineIp();
            //payload.FromIp = Helper.GetLocalMachineIp();

            workflow.TransmitData("13.81.63.14", 11000, payload, false);
        }
    }
}