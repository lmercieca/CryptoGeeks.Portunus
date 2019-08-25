using CryptoGeeks.Common;
using CryptoGeeks.Portunus.CommunicationFramework;
using CryptoGeeks.Portunus.Helpers;
using Matcha.BackgroundService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CryptoGeeks.Portunus
{
    public class PingService : IPeriodicTask
    {
        public PingService(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
        }

        public TimeSpan Interval { get; set; }

        public Task<bool> StartJob()
        {
            CryptoGeeks.Portunus.CommunicationFramework.Workflow workflow = new CryptoGeeks.Portunus.CommunicationFramework.Workflow();
            
            SecureStorage secureStorage = new SecureStorage();
            int userId = int.Parse(secureStorage.GetFromSecureStorage(Constants.UserId));

            Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, userId, "Hello Buddy");
            payload.FromIp = Helper.GetPublicMachineIp();

            workflow.TransmitData("13.81.63.14", 11000, payload);

            return Task.FromResult(true);
        }
    }

    public class TCPReceiveService : IPeriodicTask
    {
        CryptoGeeks.Portunus.CommunicationFramework.Workflow workflow = new CryptoGeeks.Portunus.CommunicationFramework.Workflow();


        public TCPReceiveService(int seconds)
        {
            Interval = TimeSpan.FromMilliseconds(seconds);
        }

        public TimeSpan Interval { get; set; }

        public Task<bool> StartJob()
        {
            JsonSerializer js = new JsonSerializer();

            var listener = Application.Current.Properties["listener"];
            
            if (listener == null)
                workflow = new CryptoGeeks.Portunus.CommunicationFramework.Workflow();
            else
                workflow = (CryptoGeeks.Portunus.CommunicationFramework.Workflow)listener;

            workflow.OnNewMessage += Workflow_OnNewMessage;
            workflow.StartListener(11000);

            Application.Current.Properties["listener"] = workflow;
            return Task.FromResult(true);
        }

        private void Workflow_OnNewMessage(object source, Payload payload, string message)
        {

            List<string> listenedList = new List<string>();

            var list = Application.Current.Properties["listenedList"];

            if (list != null)
                listenedList = (List<string>)list;

            listenedList.Add(message);

            Application.Current.Properties["listenedList"] = listenedList;
        }
    }

}
