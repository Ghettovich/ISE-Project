using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AbInitio.Web.Code
{

    public class MatchingScore
    {
        DataTable dt_personen = new DataTable();
        PersoonPartial persoon = new PersoonPartial();
        MatchPersoon m_persoon = new MatchPersoon();
        public List<MatchPersoon> list_personen { get; set; }

        public MatchingScore(int persoonid)
        {
            dt_personen = DAL.StamboomDAL.MatchPersonen(persoonid);
        }

        public DataTable Personen
        {
            get
            {
                return dt_personen;
            }
            set
            {
                value = dt_personen;
            }
        }

        public PersoonPartial Persoon
        {
            get
            {
                return persoon;
            }
            set
            {
                value = persoon;
            }
        }


        public void StartMatch()
        {

            using (IDataReader reader = dt_personen.CreateDataReader())
            {

                while (reader.Read())
                {
                    m_persoon = new MatchPersoon();
                    m_persoon.persoonid = reader.GetInt32(0);

                    if (reader.GetValue(1) != null && reader.GetString(1) == Persoon.voornaam)
                    {
                        m_persoon.ScoreVoornaam = 10;
                        m_persoon.voornaam = reader.GetString(1);
                    }
                    if (reader.GetValue(2) != null && reader.GetString(2) == Persoon.overigenamen)
                    {
                        m_persoon.ScoreOverigenamen = 10;
                        m_persoon.overigenamen = reader.GetString(2);
                    }
                    if (reader.GetValue(3) != null && reader.GetString(3) == Persoon.tussenvoegsel)
                    {
                        m_persoon.ScoreTussenvoegsel = 10;
                        m_persoon.tussenvoegsel = reader.GetString(3);
                    }
                    if (reader.GetValue(4) != null && reader.GetString(4) == Persoon.achternaam)
                    {
                        m_persoon.ScoreAchternaam = 10;
                        m_persoon.achternaam = reader.GetString(4);
                    }
                    if (reader.GetValue(5) != null && reader.GetString(5) == Persoon.achtervoegsel)
                    {
                        m_persoon.ScoreAchtervoegsel = 10;
                        m_persoon.achtervoegsel = reader.GetString(5);
                    }
                    if (reader.GetValue(6) != null && reader.GetString(6) == Persoon.geboortenaam)
                    {
                        m_persoon.ScoreGeboortenaam = 10;
                        m_persoon.geboortenaam = reader.GetString(6);
                    }
                    if (reader.GetValue(7) != null && reader.GetString(7) == Persoon.geslacht)
                    {
                        m_persoon.ScoreGeslacht = 10;
                        m_persoon.geslacht = reader.GetString(7);
                    }
                    if (reader.GetValue(8) != null && reader.GetString(8) == Persoon.geboortedatum)
                    {
                        m_persoon.ScoreGeboortedatum = 10;
                        m_persoon.geboortedatum = string.Format("{0:dd-MM-yyyy}", reader.GetValue(8));
                    }

                    //Indien match gevonden is de score groter dan 0 en dus true
                    if (m_persoon.MatchGevonden())
                    {
                        list_personen.Add(m_persoon);
                    }


                }
            }


        }        
    }
}