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
    public partial class Key
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Key()
        {
            this.Fragments = new HashSet<Fragment>();
            this.UserKeyFragments = new HashSet<UserKeyFragment>();
            this.KeyRequests = new HashSet<KeyRequest>();
        }

        public int Id { get; set; }
        public string Owner { get; set; }
        public string Key1 { get; set; }
        public int Split { get; set; }
        public Nullable<int> RecoverNo { get; set; }
        public string Data { get; set; }
        public Nullable<int> User { get; set; }
        public string DisplayName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fragment> Fragments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserKeyFragment> UserKeyFragments { get; set; }
        public virtual User User1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KeyRequest> KeyRequests { get; set; }
    }
}
