using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Models
{
    public class AanvullendPersoonModel
    {
        public int aanvullendepersooninformatieid { get; set; }
        public int persoonid { get; set; }
        public int persooninformatietypeid { get; set; }
        public string persooninformatie { get; set; }
        public string van { get; set; }
        public string tot { get; set; }
        public string datumPrecisie { get; set; }
        public DateTime gewijzigdOp { get; set; }

        public List<SelectListItem> datumPrecisies { get; set; }
        public List<SelectListItem> aanvullendPersoonInformatieTypes { get; set; }
    }
}