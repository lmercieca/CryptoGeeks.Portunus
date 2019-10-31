using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace CryptoGeeks.Portunus.Services.POCO
{
    [Preserve(AllMembers = true)]
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
