//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CryptoGeeks.API
{
    using System;
    using System.Collections.Generic;
    using Xamarin.Forms.Internals;

    [Preserve(AllMembers = true)]
    public partial class Ping
    {
        public int Id { get; set; }
        public int User_Fk { get; set; }
        public System.DateTime Time { get; set; }
        public string SourceIp { get; set; }
    }
}
