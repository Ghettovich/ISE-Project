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
        public string FamilieNaam { get; set; }
        public int stamboomid { get; set; }
        public int id { get; set; }
        public int? kekuleid { get; set; }

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
        public string KekuleID { get; set; }

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


        public bool HeeftVvg
        {
            get
            {
                if (string.IsNullOrEmpty(this.tussenvoegsel))
                {
                    return false;
                }return true;
            }
        }

        public string GeefAchternaam
        {
            get
            {
                if (HeeftVvg)
                {
                    return tussenvoegsel + " " + achternaam;
                } return achternaam;
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
        public int Totaal { get; set; }
        public int LevenshteinAfstand { get; set; }
        public int AantalAanvullendInfo { get; set; }

        public bool CheckLevenhstein()
        {
            if (ScoreVoornaam > 0 && ScoreVoornaam < 3)
            {
                return true;
            }
            if (ScoreOverigenamen > 0 && ScoreOverigenamen < 3)
            {
                return true;
            }
            if (ScoreAchternaam > 0 && ScoreAchternaam <3)
            {
                return true;
            }
            if (ScoreAchtervoegsel > 0 && ScoreAchtervoegsel < 3)
            {
                return true;
            }
            if (ScoreGeboortenaam > 0 && ScoreGeboortenaam < 3)
            {
                return true;
            } return false;
        }

        public int TotaleLevenhstein
        {
            get
            {
                return ScoreVoornaam + ScoreOverigenamen + ScoreAchternaam + ScoreAchtervoegsel + ScoreGeboortenaam;
            }
        }


    }

    
}