using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DbContexts
{
    public class AVRelatiePartial : aanvullenderelatieinformatie
    {

        public string AVInfoType { get; set; }
        public string datumVan { get; set; }
        public string datumPrecisie { get; set; }
        public string datumTot { get; set; }

    }
}