using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Services.POCO
{
    public partial class GetDashboard_Result
    {
        public Nullable<int> Keys { get; set; }
        public Nullable<int> KeyRequests { get; set; }
        public Nullable<int> Contacts { get; set; }
        public Nullable<int> Fragments { get; set; }
        public Nullable<int> FragmentRequests { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
}
