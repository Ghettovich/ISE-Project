using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Models
{
    public class PersoonModel
    {
        
        public int persoonid { get; set; }
        public string voornaam { get; set; }
        public string overigenamen { get; set; }
        public string tussenvoegsel { get; set; }
        public string achternaam { get; set; }
        public string achtervoegsel { get; set; }
        public string geboortenaam { get; set; }
        public string geslacht { get; set; }
        public string status { get; set; }
        public string geboortedatum { get; set; }
        public string geboorteprecisie { get; set; }
        public string geboortedatum2 { get; set; }

        public List<SelectListItem> geboortePrecisies { get; set; }
    }

    public class PersoonDBContext : DbContext
    {
        public DbSet<PersoonModel> Personen { get; set; }
    }
}