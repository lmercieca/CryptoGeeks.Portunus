using CryptoGeeks.Portunus.CommunicationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.PassivePeer
{
    class Program
    {
        static void Main(string[] args)
        {
            Workflow workflow = new Workflow();

            Payload payload = new Payload(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, DataType.ContactRequest,1, "Hello Buddy");

            workflow.TransmitData(Helper.GetMachineIp(), 7001, payload);


            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }

        }
    }
}
