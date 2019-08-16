using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Comm;
using CryptoGeeks.Portunus.CommunicationFramework;
using CryptoGeeks.Portunus.Models;
using CryptoGeeks.Portunus.ViewModels;
using CryptoGeeks.Portunus.Views.AddKey;
using CryptoGeeks.Portunus.Views.ExportImport;
using CryptoGeeks.Portunus.Views.Recovery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public Conversation()
        {
            InitializeComponent();

            //workflow.OnNewMessage += Workflow_OnNewMessage;
            //workflow.StartListener(Helper.GetMachineIp(), 7001);

            ItemListViewModel itemListViewModel = new ItemListViewModel();
            Task<string> userTask =  itemListViewModel.RefreshData();
            userTask.Wait();

            ContactsListView.BeginRefresh();
            Contacts =  itemListViewModel.Contacts;
            ContactsListView.EndRefresh();


            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                //Listener myserver = new Listener("192.168.***.***", 13000);
                Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, 1, "Hello Buddy");
                workflow.TransmitData("40.68.146.3", 7001, payload);
                System.Threading.Thread.Sleep(1000);

            });
        }

        private void Workflow_OnNewMessage(object source, Payload payload, string message)
        {
            txtIncoming.Text += message + Environment.NewLine;
        }

        private void BtnSend_Clicked(object sender, EventArgs e)
        {
            PayloadProcessor proc = new PayloadProcessor();

            CancellationToken canTok = new CancellationToken();
            Task<List<UserStatusCompact>> userTask = proc.GetUsersConnection(canTok);
            userTask.Wait();

            ContactViewModel cvm = (ContactViewModel)ContactsListView.SelectedItem;

            UserStatusCompact peer = (from x in userTask.Result where x.Id == cvm.Contact.Id select x).FirstOrDefault();

            if (peer != null)
            {
                Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, 1, txtMessage.Text);
                workflow.TransmitData(peer.SourceIp, 7001, payload);

                Thread.Sleep(100);
            }
        }
    }
}