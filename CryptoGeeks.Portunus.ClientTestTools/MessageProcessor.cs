using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.ClientTestTools
{
    public enum FragmentStatus
    {
        NotSet,
        Request,
        Accepted
    }

    public enum FragmentTransmissionType
    {
        NotSet,
        Request,
        Response
    }

    public enum MessageType
    {
        Handshake,
        Ping,
        FragmentSentRequest,
        FragmentReceivedRequest,
        FragmentSentResponse,
        FragmentReceivedResponse,

    }

    public class PingDataMessage
    {
        public string From { get; set; }
    }

    public class FragmentTransmissionData
    {
        private FragmentStatus status = FragmentStatus.NotSet;
        private FragmentTransmissionType type = FragmentTransmissionType.NotSet;

        public string Recepients { get; set; }
        public string Data { get; set; }
        public FragmentStatus Status { get { return status; } set { status = value; } }
        public FragmentTransmissionType Type { get { return type; } set { type = value; } }
    }

    public class SendFragmentRequestDataMessage
    {
        private List<FragmentTransmissionData> fragments = new List<FragmentTransmissionData>();

        public string From { get; set; }

        public List<FragmentTransmissionData> Fragments
        {
            get { return fragments; }
            set { fragments = value; }
        }
    }

    public class ReceiveFragmentRequestDataMessage
    {
        public string From { get; set; }

        public FragmentTransmissionData Fragment { get; set; }
    }

    public class MessageProcessor
    {
        public void ProcessMessage(Message message)
        {
            switch (message.Type)
            {/*
                case Message.MessageType.FragmentRequest:
                    break;

                case Message.MessageType.FragmentRequestResponse:
                    break;

                case Message.MessageType.Handshake:
                    break;

                case Message.MessageType.SendFragment:

                    break;

                case Message.MessageType.Ping:
                default:
                    break;
                    */
            }
        }
    }
}
