using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CruptoGeeks.Server.Host
{
    public partial class Form1 : Form
    {
        ObservableCollection<Connection> Connections = new ObservableCollection<Connection>();

        public Form1()
        {
            InitializeComponent();

            Connection item = new Connection(
                new PeerConnection()
                {
                    IP = "127.0.0.1",
                    PeerPort = 100001,
                    ServerPort = 100002
                },
                new PeerConnection()
                {
                    IP = "127.0.0.2",
                    PeerPort = 200001,
                    ServerPort = 200002
                }
            );

            Connections.Add(item);
            UpdateDisplay();
         
        }

        private void UpdateDisplay()
        {
            var displayConnections = Connections.Select(x => new
            {
                PrimaryIP = x.Primary.IP,
                PrimaryPeerPort = x.Primary.PeerPort,
                PrimaryServerPort = x.Primary.ServerPort,
                SecondaryServerPort = x.Secondary.ServerPort,
                SecondaryIP = x.Secondary.IP,
                SecondaryPort = x.Secondary.PeerPort
            }).ToList();

            dgvDetails.DataSource = displayConnections;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection item = new Connection(
               new PeerConnection()
               {
                   IP = "127.0.0.3",
                   PeerPort = 300001,
                   ServerPort = 300002
               },
               new PeerConnection()
               {
                   IP = "127.0.0.4",
                   PeerPort = 400001,
                   ServerPort = 400002
               }
           );

            Connections.Add(item);
            UpdateDisplay();
        }
    }

    public class PeerConnection
    {
        public string IP { get; set; }
        public int PeerPort { get; set; }
        public int ServerPort { get; set; }
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
