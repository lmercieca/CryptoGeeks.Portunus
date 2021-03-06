﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace CryptoGeeks.Portunus.Services.POCO
{
    [Preserve(AllMembers = true)]
    public partial class GetFragmentsForKey_Result
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
