using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Models;
using Moserware.Security.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoGeeks.Portunus.Comm
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
        public string Key { get; set; }
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
        SecureStorage secureStorage = new SecureStorage();

        public Message PrepareMessageForFragmentsSend(Key k)
        {
            Message message = new Message();
            message.DisplayName = secureStorage.GetFromSecureStorage(Constants.DisplayName);
            message.Type = MessageType.FragmentReceivedRequest;

            SendFragmentRequestDataMessage frags = new SendFragmentRequestDataMessage();

            foreach (Fragment f in k.Fragments)
            {
                frags.Fragments.Add(new FragmentTransmissionData()
                {
                    Data = f.Data,
                    Recepients = f.Owner.DisplayName,
                    Key = f.Identifier,
                    Status = FragmentStatus.NotSet,
                    Type = FragmentTransmissionType.NotSet
                });
            }
            message.Data = JsonConvert.SerializeObject(frags);

            return message;
        }

        public string ProcessMessage(Message message)
        {
            FragmentTransmissionData payLoad;
            string fragmentsJson;
            List<Fragment> fragments;
            string savingResult;

            switch (message.Type)
            {
                case MessageType.FragmentReceivedRequest:
                    SendFragmentRequestDataMessage fragmentsToSend = JsonConvert.DeserializeObject<SendFragmentRequestDataMessage>(message.Data);

                    foreach (FragmentTransmissionData tempFrag in fragmentsToSend.Fragments)
                    {
                        message.Data = JsonConvert.SerializeObject(tempFrag);
                        Transmitter.SendData(Constants.ServerIP, Constants.DefaultServerPort, message);
                    }

                    break;

                //Of course I will store this fragment for you
                case MessageType.FragmentReceivedResponse:

                    payLoad = JsonConvert.DeserializeObject<FragmentTransmissionData>(message.Data);
                    Fragment frag = new Fragment
                    {
                        Data = payLoad.Data,
                        Identifier = payLoad.Key
                    };

                    fragmentsJson = secureStorage.GetFromSecureStorage(Constants.OtherUsersFragmentsList);
                    fragments = JsonConvert.DeserializeObject<List<Fragment>>(fragmentsJson);
                    fragments.Add(frag);

                    savingResult = JsonConvert.SerializeObject(fragments);
                    secureStorage.StoreInSecureStorage(Constants.OtherUsersFragmentsList, savingResult);

                    break;

                //Can you please give me back my fragment?
                case MessageType.FragmentSentRequest:
                    payLoad = JsonConvert.DeserializeObject<FragmentTransmissionData>(message.Data);
                    fragmentsJson = secureStorage.GetFromSecureStorage(Constants.OtherUsersFragmentsList);
                    fragments = JsonConvert.DeserializeObject<List<Fragment>>(fragmentsJson);

                    Fragment fragToSend = (from f in fragments where f.Identifier == payLoad.Key select f).FirstOrDefault();

                    if (fragToSend != null)
                    {
                        payLoad.Data = fragToSend.Data;

                        //Remove it locally once sent back
                        fragments.Remove(fragToSend);
                        savingResult = JsonConvert.SerializeObject(fragments);
                        secureStorage.StoreInSecureStorage(Constants.OtherUsersFragmentsList, savingResult);

                        //Send the info back
                        message.DisplayName = secureStorage.GetFromSecureStorage(Constants.DisplayName);
                        message.Type = MessageType.FragmentSentResponse;
                        message.Data = JsonConvert.SerializeObject(payLoad);

                        Transmitter.SendData(Constants.ServerIP, message.DedicatedPort, message);
                    }

                    break;

                // Thanks for the fragment you sent me back
                case MessageType.FragmentSentResponse:
                    payLoad = JsonConvert.DeserializeObject<FragmentTransmissionData>(message.Data);
                    fragmentsJson = secureStorage.GetFromSecureStorage(Constants.KeysList);
                    List<Key> keys = JsonConvert.DeserializeObject<List<Key>>(fragmentsJson);

                    Key keyStored = (from k in keys where k.Identifier == payLoad.Key select k).FirstOrDefault();
                    keyStored.Fragments.Add(new Fragment()
                    { Data = payLoad.Data });

                    if (keyStored.Fragments.Count >= keyStored.RequiredFragments)
                    {
                        string[] shares = (from s in keyStored.Fragments select s.Data).ToArray();

                        CombinedSecret result = SecretCombiner.Combine(shares);

                        keys.Remove(keyStored);
                        string updatedKeyList = JsonConvert.SerializeObject(keys);
                        secureStorage.StoreInSecureStorage(Constants.KeysList, updatedKeyList);

                        return result.RecoveredTextString;
                    }


                    break;

                default:
                    break;
            }

            return string.Empty;
        }
    }
}
