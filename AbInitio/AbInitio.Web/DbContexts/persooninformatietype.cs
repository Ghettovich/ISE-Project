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
    
    public partial class persooninformatietype
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public persooninformatietype()
        {
            this.aanvullendepersooninformaties = new HashSet<aanvullendepersooninformatie>();
        }
    
        public int persooninformatietypeid { get; set; }
        public string persooninformatietype1 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<aanvullendepersooninformatie> aanvullendepersooninformaties { get; set; }
    }
}
