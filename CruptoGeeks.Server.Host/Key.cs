//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CruptoGeeks.Server.Host
{
    using System;
    using System.Collections.Generic;
    
    public partial class Key
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Key()
        {
            this.Fragments = new HashSet<Fragment>();
        }
    
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Key1 { get; set; }
        public int Split { get; set; }
        public Nullable<int> RecoverNo { get; set; }
        public string Data { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fragment> Fragments { get; set; }
    }
}
