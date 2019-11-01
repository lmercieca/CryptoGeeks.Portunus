using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace CryptoGeeks.Portunus.Services.POCO
{

    [Preserve(AllMembers = true)]
    public partial class GetKeyRequestsForKey_Result
    {
        public int Id { get; set; }
        public Nullable<int> KeyID { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<bool> Completed { get; set; }
    }
}
