using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Models
{

    public class PersoonInStamboom
    {
        public string persoonInStamboomId { get; set; }
        public string familieNaam { get; set; }
        public string persoonId { get; set; }
        public string kekuleId { get; set; }
        public string voornaam { get; set; }
        public string achternaam { get; set; }
        public string tussenvoegsel { get; set; }
        public string geboortePrecisie { get; set; }
        public Nullable<System.DateTime> geboortedatum { get; set; }
        public Nullable<System.DateTime> geboortedatum2 { get; set; }
        public string trouwdatum { get; set; }
        public string overlijdingsdatum { get; set; }
    }

    public class StamboomModel
    {
        public int accountId { get; set; }
        public int stamboomId { get; set; }
        public string familieNaam { get; set; }
        public DateTime gewijzigdOp { get; set; }
    }
}