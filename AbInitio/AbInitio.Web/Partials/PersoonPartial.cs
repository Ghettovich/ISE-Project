using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DbContexts
{
    public class PersoonPartial : persoon
    {
        public new string status { get; set; }
        public new string geboortedatum { get; set; }
        public new string geboortedatum2 { get; set; }

        public string RelatieType { get; set; }
        public int RelatieID { get; set; }
        
            
        public string GeefGeslacht
        {
            get
            {
                if (!string.IsNullOrEmpty(geslacht))
                {
                    if (geslacht == "M")
                    {
                        return "Man";
                    } return "Vrouw";
                } return "Onbekend";
            }
        }

        public string GeefVolledigeNaam
        {
            get
            {
                return string.Format("{0} {1}{2}", voornaam, (tussenvoegsel != string.Empty ? tussenvoegsel + " " : ""), achternaam);
            }
        }

        public string geefStatus
        {
            get
            {
                if (status == "1")
                {
                    return "Levend";
                } return "Overleden";
            }
        }

    }
}