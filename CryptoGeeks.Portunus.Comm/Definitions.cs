using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CryptoGeeks.Portunus.CommunicationFramework
{
    
    public enum MessageType { NewConnection, Ping, RequestForOpen, RequestForChannel, RequestForClose, PeerMessage, Hello, CloseServerConnection, ClosePeerConnection };
    public enum MessageSource { ActivePeer, PassivePeer, Server }
    public enum MessageState { Request, Response }
    public enum DataType { ClientPortResponse, ContactRequest, RequestForHoldFragment, RequestForReturnFragment }


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


          //  File.WriteAllText(filename, data);


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
            string resp = new WebClient().DownloadString("https://portunus.azurewebsites.net/api/helper/getmyip");

            return resp.Replace("\"", "");
        }

        public static MemoryStream PreparePayloadForSending(Payload message)
        {
            MemoryStream msChannel = new MemoryStream();
                        
            byte[] serializedPayload = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message));

            msChannel.Write(serializedPayload, 0, serializedPayload.Length);

            //Serializer.Serialize<Payload>(msChannel, message);


            string eof = "<eof>";
            byte[] eofBytes = Encoding.ASCII.GetBytes(eof);
            msChannel.Write(eofBytes, 0, eofBytes.Length);

            //msChannel.Position = msChannel.Length - 1;

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

    [ProtoContract(SkipConstructor = true)]
    public class CoreMessage
    {
        [ProtoMember(1)]
        [JsonProperty(Required = Required.Always)]
        public MessageType MessageType { get; private set; }

        [JsonProperty(Required = Required.Always)]
        [ProtoMember(2)]
        public MessageSource MessageSource { get; private set; }

        [ProtoMember(3)]
        [JsonProperty(Required = Required.Always)]
        public MessageState MessageState { get; set; }

        [ProtoMember(4)]
        [JsonProperty(Required = Required.Always)]
        public string FromIp { get; set; }

        [ProtoMember(5)]
        [JsonProperty(Required = Required.Always)]
        public int OwnerUserId { get; set; }


   

        public CoreMessage(MessageType type, MessageSource source, MessageState state, int ownerUserId)
        {
            this.MessageType = type;
            this.MessageSource = source;
            this.MessageState = state;

            this.FromIp = Helper.GetPublicMachineIp();
            this.OwnerUserId = ownerUserId;
        }
    }

    [ProtoContract(SkipConstructor = true)]
    public class Payload : CoreMessage
    {

        [ProtoMember(11)]
        [JsonProperty(Required = Required.Always)]
        public DataType DataType { get; set; }

        [ProtoMember(12)]
        [JsonProperty(Required = Required.Always)]
        public string PayloadData { get; set; }

        [ProtoMember(13)]
        [JsonProperty(Required = Required.Always)]
        public string AdditionalData { get; set; }


        public Payload(MessageType type, MessageSource source, MessageState state, DataType dataType, int ownerUserId, string payloadData, string additionalData = "") : base(type, source, state, ownerUserId)
        {
            this.DataType = DataType;
            this.PayloadData = payloadData;
            this.AdditionalData = additionalData;
        }
    }
}
