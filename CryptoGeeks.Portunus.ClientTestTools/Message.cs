using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.ClientTestTools
{
    public class Message
    {
    

        /*
        public string FromIp { get; set; }

        public string ToIp { get; set; }
     
        public string To { get; set; }
        */

        public MessageType Type { get; set; }

        public string Data { get; set; }

        public string DisplayName { get; set; }
    }
}

