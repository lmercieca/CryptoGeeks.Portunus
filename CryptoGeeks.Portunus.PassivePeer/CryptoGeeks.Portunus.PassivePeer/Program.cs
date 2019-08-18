using CryptoGeeks.Portunus.Comm;
using CryptoGeeks.Portunus.CommunicationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.PassivePeer
{
    class Program
    {
        static void Main(string[] args)
        {
            Workflow workflow = new Workflow();

            workflow.OnNewMessage += Workflow_OnNewMessage;
            workflow.StartListener(Helper.GetLocalMachineIp(), 11000);

            Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, 1, "Hello Buddy");
            payload.FromIp = Helper.GetPublicMachineIp();

            //workflow.TransmitData(Helper.GetMachineIp(), 11000, payload);
            workflow.TransmitData("13.81.63.14", 11000, payload);
            return;


            System.Threading.Thread.Sleep(1000);

            PayloadProcessor proc = new PayloadProcessor();

            Task<bool> res = proc.MarkPing(payload);

            //Get peer ping
            CancellationToken canTok = new CancellationToken();
            Task<List<UserStatusCompact>> userTask = proc.GetUsersConnection(canTok);
            userTask.Wait();


            UserStatusCompact peer = (from x in userTask.Result where x.Id == 1 select x).FirstOrDefault();

            if (peer != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest, 1, "Hello Buddy");
                    workflow.TransmitData(peer.SourceIp, 11000, payload);

                    Thread.Sleep(100);
                }
            }



            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }

        }

        private static void Workflow_OnNewMessage(object source, Payload payload, string message)
        {
            Console.WriteLine("New message from " + source.GetType() + " saying " + message);
        }
    }
}
