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
        public int stamboomid { get; set; }
        public int id { get; set; }

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

        public string geefDatum
        {
            get 
            {
                if(geboortedatum != null && geboortedatum != "")
                {
                    DateTime d = DateTime.Parse(geboortedatum);
                    return d.ToString("dd-MM-yyyy");
                } else
                {
                    return "";
                }
                
            }
            
        }

        public string geefDatum2
        {
            get
            {
                if(geboortedatum2 != null && geboortedatum2 != "")
                {
                    DateTime d = DateTime.Parse(geboortedatum2);
                    return d.ToString("dd-MM-yyyy");
                } else
                {
                    return "";
                }
                
            }

        }

    }

    public class MatchPersoon : PersoonPartial
    {
        public int ScoreVoornaam { get; set; }
        public int ScoreOverigenamen { get; set; }
        public int ScoreTussenvoegsel { get; set; }
        public int ScoreAchternaam { get; set; }
        public int ScoreAchtervoegsel { get; set; }
        public int ScoreGeboortenaam { get; set; }
        public int ScoreGeslacht { get; set; }
        public int ScoreStatus { get; set; }
        public int ScoreGeboortedatum { get; set; }


        public int TotaalScore()
        {
            return ScoreAchternaam + ScoreAchtervoegsel + ScoreGeboortedatum + ScoreGeboortenaam + ScoreGeslacht + ScoreOverigenamen + ScoreStatus + ScoreTussenvoegsel + ScoreVoornaam;




        }

        public bool MatchGevonden()
        {
            if (ScoreVoornaam > 0)
            {
                return true;
            }
            if (ScoreOverigenamen > 0)
            {
                return true;
            }
            if (ScoreTussenvoegsel > 0)
            {
                return true;
            }
            if (ScoreAchternaam > 0)
            {
                return true;
            }
            if (ScoreAchtervoegsel > 0)
            {
                return true;
            }
            if (ScoreGeboortenaam > 0)
            {
                return true;
            }
            if (ScoreGeslacht > 0)
            {
                return true;
            }
            if (ScoreStatus > 0)
            {
                return true;
            }
            if (ScoreGeboortedatum > 0)
            {
                return true;
            } return false;
        }

    }
}