using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    public enum MessageType { Ping, RequestForOpen, RequestForChannel };
    public enum MessageSource { ActivePeer, PassivePeer, Server }
    public enum MessageState { Request, Response }
    public enum DataType { ContactRequest, RequestForHoldFragment, RequestForReturnFragment }


    public class LoggerHelper
    {
        public static void DeleteFile(string data) 
        {
            var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "portunus.log");

            File.Delete(filename);
        }

        public static void AddLog(string data) // Code to generate a text file
        {
            var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "portunus.log");


            File.WriteAllText(filename, data);


        }
    }

    public class Helper
    {
        public static string GetLocalMachineIp()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }
        public static string GetPublicMachineIp()
        {
            return new WebClient().DownloadString("http://icanhazip.com").Replace("\n", "");
        }

        public static MemoryStream PreparePayloadForSending(Payload message)
        {
            MemoryStream msChannel = new MemoryStream();
            Serializer.Serialize<Payload>(msChannel, message);


            string eof = "<eof>";
            byte[] eofBytes = Encoding.ASCII.GetBytes(eof);
            msChannel.Write(eofBytes, 0, eofBytes.Length);


            long size = 8;
            while (msChannel.Length % size != 0)
            {
                msChannel.Write(new byte[] { 0 }, 0, 1);
            }


            msChannel.Position = 0;
            return msChannel;
        }

        public static MemoryStream CleanIncomingStream(MemoryStream memStream)
        {
            memStream.Position = 0;
            byte[] workingBytes = new byte[memStream.Length];
            memStream.Read(workingBytes, 0, workingBytes.Length);

            List<byte> workingBytesAsList = new List<byte>(workingBytes);
            bool trainingSpacesPresent = true;

            for (int i = workingBytes.Length - 1; i >= 0; i--)
            {

                if (workingBytes[i] == 0 && trainingSpacesPresent)
                    workingBytesAsList.RemoveAt(i);
                else
                {
                    trainingSpacesPresent = false;
                    workingBytesAsList.RemoveRange(workingBytesAsList.Count - 5, 5);
                    break;
                }
            }


            MemoryStream cleanedStream = new MemoryStream(workingBytesAsList.ToArray());
            return cleanedStream;
        }

        public static int SendPayload(NetworkStream stream, Payload payload)
        {
            int bytesread;
            MemoryStream msReturn = Helper.PreparePayloadForSending(payload);


            byte[] resp = new byte[8];
            bytesread = msReturn.Read(resp, 0, resp.Length);


            while (bytesread > 0)
            {
                stream.Write(resp, 0, bytesread);
                bytesread = msReturn.Read(resp, 0, resp.Length);
            }

            return bytesread;
        }

        public static MemoryStream ReceiveStream(NetworkStream stream)
        {
            byte[] resp = new byte[8];
            var memStream = new MemoryStream();

            int bytesread = stream.Read(resp, 0, resp.Length);
            bool complete = false;

            string eof = "<eof>";
            string incomingData = "";

            while (!complete)
            {
                memStream.Write(resp, 0, bytesread);
                bytesread = stream.Read(resp, 0, resp.Length);

                incomingData += Encoding.ASCII.GetString(resp);

                complete = (incomingData.Contains(eof));
            }
            memStream.Write(resp, 0, bytesread);
            return memStream;
        }

        public static string PrintPayload(Payload payload)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("MessageType: " + payload.MessageType + Environment.NewLine);
            sb.Append("MessageSource: " + payload.MessageSource + Environment.NewLine);
            sb.Append("MessageState: " + payload.MessageState + Environment.NewLine);
            sb.Append("FromIp: " + payload.FromIp + Environment.NewLine);
            sb.Append("DataType: " + payload.DataType + Environment.NewLine);
            sb.Append("PayloadData: " + payload.PayloadData + Environment.NewLine);

            return sb.ToString();
        }
    }

    [ProtoContract]
    public class CoreMessage
    {
        [ProtoMember(1)]
        public MessageType MessageType { get; private set; }
        [ProtoMember(2)]
        public MessageSource MessageSource { get; private set; }
        [ProtoMember(3)]
        public MessageState MessageState { get; private set; }

        [ProtoMember(4)]
        public string FromIp { get; set; }

        [ProtoMember(5)]
        public int OwnerUserId { get; set; }


        public CoreMessage()
        {
            this.MessageType = MessageType.Ping;
            this.MessageSource = MessageSource.ActivePeer;
            this.MessageState = MessageState.Request;

            this.FromIp = Helper.GetPublicMachineIp();
        }

        public CoreMessage(MessageType type, MessageSource source, MessageState state, int ownerUserId)
        {
            this.MessageType = type;
            this.MessageSource = source;
            this.MessageState = state;

            this.FromIp = Helper.GetPublicMachineIp();
            this.OwnerUserId = ownerUserId;
        }
    }

    [ProtoContract]
    public class Payload : CoreMessage
    {
        [ProtoMember(11)]
        public DataType DataType { get; private set; }
        [ProtoMember(12)]
        public string PayloadData { get; set; }

        public Payload() : base(MessageType.Ping, MessageSource.ActivePeer, MessageState.Request, 1)
        {

        }

        public Payload(MessageType type, MessageSource source, MessageState state, DataType dataType, int ownerUserId, string payloadData) : base(type, source, state, ownerUserId)
        {
            this.DataType = DataType;
            this.PayloadData = payloadData;
        }
    }

}
