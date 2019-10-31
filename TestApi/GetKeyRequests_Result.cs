using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Services.POCO
{
    public partial class GetKeyRequests_Result
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Key1 { get; set; }
        public int Split { get; set; }
        public Nullable<int> RecoverNo { get; set; }
        public string Data { get; set; }
        public Nullable<int> User { get; set; }
        public string DisplayName { get; set; }
        public Nullable<int> fragsReceived { get; set; }
        public Nullable<int> totalFrags { get; set; }
        public string color { get; set; }
        public string description { get; set; }
    }
}
