using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Models
{
    public class NieuwPersoonModel
    {

        public string voornaam { get; set; }
        public string overigenamen { get; set; }
        public string tussenvoegsel { get; set; }
        public string achternaam { get; set; }        
        public string achtervoegsel { get; set; }
        public string geboortenaam { get; set; }
        public string geslacht { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<System.DateTime> geboortedatum { get; set; }        
        public SelectList GeboortePrecisie { get; set; }
        public string geboorteprecisie { get; set; }
        public Nullable<System.DateTime> geboortedatum2 { get; set; }
        public string geboorteplaats { get; set; }
        public string adress { get; set; }
        public string beroep { get; set; }

    }

    public class WijzigPersoonModel
    {
        

    }
}