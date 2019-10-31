using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Services.POCO
{
    public partial class GetKeysForUser_Result
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string DisplayName { get; set; }
        public string Owner { get; set; }
        public Nullable<int> RecoverNo { get; set; }
        public int Split { get; set; }
        public Nullable<int> User { get; set; }
    }
}
