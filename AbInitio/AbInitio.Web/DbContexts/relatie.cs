//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AbInitio.Web.DbContexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class relatie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public relatie()
        {
            this.aanvullenderelatieinformaties = new HashSet<aanvullenderelatieinformatie>();
            this.relatiehistories = new HashSet<relatiehistorie>();
        }
    
        public int relatieid { get; set; }
        public int persoonid1 { get; set; }
        public int relatietypeid { get; set; }
        public int persoonid2 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<aanvullenderelatieinformatie> aanvullenderelatieinformaties { get; set; }
        public virtual persoon persoon { get; set; }
        public virtual persoon persoon1 { get; set; }
        public virtual relatietype relatietype { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<relatiehistorie> relatiehistories { get; set; }
    }
}
