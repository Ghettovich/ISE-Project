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
    
    public partial class aanvullendepersooninformatie
    {
        public int aanvullendepersooninformatieid { get; set; }
        public int persoonid { get; set; }
        public int persooninformatietypeid { get; set; }
        public string persooninformatie { get; set; }
        public Nullable<System.DateTime> van { get; set; }
        public Nullable<System.DateTime> tot { get; set; }
        public string datumPrecisie { get; set; }
        public System.DateTime gewijzigdOp { get; set; }
    
        public virtual persoon persoon { get; set; }
        public virtual persooninformatietype persooninformatietype { get; set; }
    }
}
