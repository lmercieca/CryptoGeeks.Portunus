using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace CryptoGeeks.Portunus.Services.POCO
{
    [Preserve(AllMembers = true)]
    public partial class GetAvailableContactsForUser_Result
    {
        public string DisplayName { get; set; }
        public int UserId { get; set; }
    }
}
