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
        public string Precisie { get; set; }
        public string datumTot { get; set; }



        public string geefDatum
        {
            get
            {
                if ((datumPrecisie == "op"
                    || datumPrecisie == "voor"
                    || datumPrecisie == "na") && !string.IsNullOrEmpty(datumVan))
                {
                    return datumPrecisie + " " + datumVan;

                }
                else if (datumPrecisie == "tussen")
                {
                    return datumPrecisie + " " + datumVan + " en "
                        + datumTot;
                } return "";
            }
        }



    }
}