using CryptoGeeks.Portunus.CommunicationFramework;
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

        public async Task<bool> StartJob()
        {
            Workflow workflow = new Workflow();

            Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, 1, "Hello Buddy");
            workflow.TransmitData("40.68.146.3", 7001, payload);

            return await Task.FromResult(true);
        }
    }

    public class TCPReceiveService : IPeriodicTask
    {
        Workflow workflow = new Workflow();


        public TCPReceiveService(int seconds)
        {
            Interval = TimeSpan.FromMilliseconds(seconds);
        }

        public TimeSpan Interval { get; set; }

        public async Task<bool> StartJob()
        {
            JsonSerializer js = new JsonSerializer();

            var listener = Application.Current.Properties["listener"];
            
            if (listener == null)
                workflow = new Workflow();
            else
                workflow = (Workflow)listener;

            workflow.OnNewMessage += Workflow_OnNewMessage;
            workflow.StartListener(Helper.GetMachineIp(), 7001);

            Application.Current.Properties["listener"] = workflow;
            return await Task.FromResult(true);
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
