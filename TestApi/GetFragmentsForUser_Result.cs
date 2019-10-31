using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Services.POCO
{
    public partial class GetFragmentsForUser_Result
    {
        public int Id { get; set; }
        public int KeyFk { get; set; }
        public string FragmentHolder { get; set; }
        public bool SentToHolder { get; set; }
        public bool SentToOwner { get; set; }
        public string Data { get; set; }
        public Nullable<int> Owner { get; set; }
    }
}
