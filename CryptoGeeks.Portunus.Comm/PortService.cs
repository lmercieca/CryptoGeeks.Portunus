using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Comm
{
    public class PortService
    {
        object _lock = new object();

        List<int> AvailablePorts = new List<int>();
        private static PortService instance = null;

        public static PortService GetInstance()
        {
            if (instance == null)
                instance = new PortService();

            return instance;
        }

        private PortService()
        {
            Task t = Task.Factory.StartNew(() =>
            {
                Parallel.Invoke(() => RefreshPortListProcess());
            });

        }

        public int GetNextPort()
        {
            return AvailablePorts.First();
        }


        public void RefreshPortListProcess()
        {
            while (true)
            {
                RefreshPortList(1000);
                Thread.Sleep(100);
            }
        }

        private void RefreshPortList(int startingPort)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            var tcpConnectionPorts = properties.GetActiveTcpConnections()
                                .Where(n => n.LocalEndPoint.Port >= startingPort)
                                .Select(n => n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            var tcpListenerPorts = properties.GetActiveTcpListeners()
                                .Where(n => n.Port >= startingPort)
                                .Select(n => n.Port);

            ////getting active udp listeners
            //var udpListenerPorts = properties.GetActiveUdpListeners()
            //                    .Where(n => n.Port >= startingPort)
            //                    .Select(n => n.Port);

            if (Monitor.TryEnter(_lock, 300))
            {
                AvailablePorts = Enumerable.Range(startingPort, ushort.MaxValue)
                    .Where(i => !tcpConnectionPorts.Contains(i))
                    .Where(i => !tcpListenerPorts.Contains(i)).ToList();
                //.Where(i => !udpListenerPorts.Contains(i)).ToList();

                Monitor.Exit(_lock);
            }

            return;

        }
    }

}
