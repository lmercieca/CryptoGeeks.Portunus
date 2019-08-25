using CryptoGeeks.Portunus.CommunicationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Server
{
    class Program
    {
        static void Main(string[] args)
        {

            
            Workflow workflow = new Workflow();

            workflow.StartListener(11000);

            workflow.OnNewMessage += Workflow_OnNewMessage;

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void Workflow_OnNewMessage(object source, Payload payload, string message)
        {
            Console.WriteLine(message);
        }
    }
}
