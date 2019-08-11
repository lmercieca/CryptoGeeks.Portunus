using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    public class Workflow
    {
        public void StartListener(string ip, int port)
        {
            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                //Listener myserver = new Listener("192.168.***.***", 13000);
                Listener myserver = new Listener(ip, port);
            });
            t.Start();

            Console.WriteLine("Server Started on " + ip + ":" + port);
        }

        public void TransmitData(string serverIp, int port, Payload payload)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Transmitter transmitter = new Transmitter();
                //transmitter.Connect("192.168.***.***", payload);
                transmitter.Connect(serverIp, port, payload);
            }).Start();
        }
    }
}
