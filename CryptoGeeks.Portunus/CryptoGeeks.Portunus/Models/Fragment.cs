using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Models
{
    public class Fragment
    {
        public Key Parent { get; set; }
        public Contact Owner { get; set; }
        public string Data { get; set; }
    }
}
