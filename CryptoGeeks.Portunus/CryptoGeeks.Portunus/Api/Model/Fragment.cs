using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Api.Model
{
    public partial class ApiFragment
    {
        public int Id { get; set; }
        public int KeyFk { get; set; }
        public string FragmentHolder { get; set; }
        public bool SentToHolder { get; set; }
        public bool SentToOwner { get; set; }
        public string Data { get; set; }
        public Nullable<int> Owner { get; set; }

        public virtual ApiKey Key { get; set; }
    }
}
