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
    
    public partial class persoon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public persoon()
        {
            this.aanvullendepersooninformaties = new HashSet<aanvullendepersooninformatie>();
            this.personeninstambooms = new HashSet<personeninstamboom>();
            this.persoonhistories = new HashSet<persoonhistorie>();
            this.relaties = new HashSet<relatie>();
            this.relaties1 = new HashSet<relatie>();
        }
    
        public int persoonid { get; set; }
        public string voornaam { get; set; }
        public string overigenamen { get; set; }
        public string tussenvoegsel { get; set; }
        public string achternaam { get; set; }
        public string achtervoegsel { get; set; }
        public string geboortenaam { get; set; }
        public string geslacht { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<System.DateTime> geboortedatum { get; set; }
        public string geboorteprecisie { get; set; }
        public Nullable<System.DateTime> geboortedatum2 { get; set; }
        public System.DateTime gewijzigdOp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<aanvullendepersooninformatie> aanvullendepersooninformaties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<personeninstamboom> personeninstambooms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<persoonhistorie> persoonhistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<relatie> relaties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<relatie> relaties1 { get; set; }
    }
}
