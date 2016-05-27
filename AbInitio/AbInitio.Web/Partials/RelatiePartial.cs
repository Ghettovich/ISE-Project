using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DbContexts
{
    public class RelatiePartial : relatie
    {

        public int AvrRelatieID { get; set; }
        public string RelatieType { get; set; }
        public string RelatieInformatie { get; set; }

        public List<aanvullenderelatieinformatie> AanvullendeInfo { get; set; }



    }
}