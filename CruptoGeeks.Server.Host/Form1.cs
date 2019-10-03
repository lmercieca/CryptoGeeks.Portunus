using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CruptoGeeks.Server.Host
{
    public partial class Form1 : Form
    {
        ObservableCollection<Connection> Connections = new ObservableCollection<Connection>();
        const string SERVER_IP = "127.0.0.1";
        MainChannel mc = new MainChannel();

        public Form1()
        {
            InitializeComponent();

            nudPort.Maximum = int.MaxValue;

            mc.OnNewClient += Mc_OnNewClient;

            //Connection item = new Connection(
            //    new PeerConnection()
            //    {
            //        IP = "127.0.0.1",
            //        PeerPort = 100001,
            //        ServerPort = 100002
            //    },
            //    new PeerConnection()
            //    {
            //        IP = "127.0.0.2",
            //        PeerPort = 200001,
            //        ServerPort = 200002
            //    }
            //);

            //Connections.Add(item);
            //UpdateDisplay();



        }

        private void Mc_OnNewClient(string remoteIp, int remotePort, string serverIp, int serverPort, int userId)
        {
            Connection conn = new Connection(
                new PeerConnection()
                {
                    IP = remoteIp,
                    PeerPort = remotePort,
                    ServerPort = serverPort,
                    UserID = userId
                },
                new PeerConnection()
                {
                    IP = "",
                    PeerPort = -1,
                    ServerPort = -1,
                    UserID = -1
                }
            );

            Connections.Add(conn);

            this.Invoke((MethodInvoker)delegate { UpdateDisplay(); });

        }

        private void UpdateDisplay()
        {
            var displayConnections = Connections.Select(x => new
            {
                PrimaryIP = x.Primary.IP,
                PrimaryPeerPort = x.Primary.PeerPort,
                PrimaryServerPort = x.Primary.ServerPort,
                PrimaryUser = x.Primary.UserID,
                SecondaryServerPort = x.Secondary.ServerPort,
                SecondaryIP = x.Secondary.IP,
                SecondaryPort = x.Secondary.PeerPort,
                SecUser = x.Secondary.UserID
            }).ToList();

            dgvDetails.DataSource = displayConnections;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                mc.StartListening((int)nudPort.Value);
               
            });
        }
    }

    public class PeerConnection
    {
        public string IP { get; set; }
        public int PeerPort { get; set; }
        public int ServerPort { get; set; }
        public int UserID { get; set; }
    }

    public class Connection
    {
        public PeerConnection Primary { get; set; }
        public PeerConnection Secondary { get; set; }

        public Connection(PeerConnection primary, PeerConnection secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }
    }
}
