using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Api.Model
{
    public partial class ApiKey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApiKey()
        {
            this.Fragments = new HashSet<ApiFragment>();
        }

        public int Id { get; set; }
        public string Owner { get; set; }
        public string Key1 { get; set; }
        public int Split { get; set; }
        public Nullable<int> RecoverNo { get; set; }
        public string Data { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApiFragment> Fragments { get; set; }
    }
}
