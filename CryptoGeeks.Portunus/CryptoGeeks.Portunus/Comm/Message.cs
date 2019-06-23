using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Comm
{
    public class Message
    {
        /*
        public string FromIp { get; set; }

        public string ToIp { get; set; }
     
        public string To { get; set; }
        */

        public int DedicatedPort { get; set; }

        public MessageType Type { get; set; }

        public string Data { get; set; }

        public string DisplayName { get; set; }
    }
}
