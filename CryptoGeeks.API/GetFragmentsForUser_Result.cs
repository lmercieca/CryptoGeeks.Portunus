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
