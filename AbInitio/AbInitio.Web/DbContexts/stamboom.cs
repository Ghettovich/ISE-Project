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
    
    public partial class stamboom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public stamboom()
        {
            this.meestvoorkomendenamen = new HashSet<meestvoorkomendenaman>();
            this.personeninstambooms = new HashSet<personeninstamboom>();
            this.stamboomtoegangs = new HashSet<stamboomtoegang>();
        }
    
        public int stamboomid { get; set; }
        public string familienaam { get; set; }
        public Nullable<int> levensverwachtingman { get; set; }
        public Nullable<int> levensverwachtingvrouw { get; set; }
        public Nullable<int> langstlevendeman { get; set; }
        public Nullable<int> langstlevendevrouw { get; set; }
        public Nullable<int> jongstlevendeman { get; set; }
        public Nullable<int> jongstlevendevrouw { get; set; }
        public Nullable<int> gemiddeldaantalkinderen { get; set; }
        public Nullable<int> gemiddeldaantalgeboortes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<meestvoorkomendenaman> meestvoorkomendenamen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<personeninstamboom> personeninstambooms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stamboomtoegang> stamboomtoegangs { get; set; }
    }
}
