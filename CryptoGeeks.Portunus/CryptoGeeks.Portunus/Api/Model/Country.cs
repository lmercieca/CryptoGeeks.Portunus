using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Api.Model
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso2 { get; set; }
        public string CallingCode { get; set; }
    }
}
