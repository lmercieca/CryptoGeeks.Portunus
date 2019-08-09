using System;
using System.Net;
using System.Net.Sockets;

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    public enum MessageType { Ping, PingResponse, RequestForOpen, RequestForClose };
    public enum MessageSource { ActivePeer, PassivePeer, Server }
    public enum MessageState { Request, Response }
    public enum DataType { ContactRequest, RequestForHoldFragment, RequestForReturnFragment }

    public class Ping
    {
        public MessageType MessageType { get; private set; }
        public MessageSource MessageSource { get; private set; }
        public MessageState MessageState { get; private set; }


        public string FromIp { get; set; }
        public DateTime Timestamp { get; set; }

        public Ping(MessageType type, MessageSource source, MessageState state)
        {
            this.MessageType = type;
            this.MessageSource = source;
            this.MessageState = state;
                       
            Timestamp = DateTime.Now;

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                FromIp = endPoint.Address.ToString();
            }        
        }
    }

    public class Payload : Ping
    {
        public DataType DataType { get; private set; }
        public string PayloadData { get; set; }

        public Payload(MessageType type, MessageSource source, MessageState state, DataType dataType, string payloadData) : base(type, source, state)
        {
            this.DataType = DataType;
            this.PayloadData = payloadData;
        }

    }    

}
