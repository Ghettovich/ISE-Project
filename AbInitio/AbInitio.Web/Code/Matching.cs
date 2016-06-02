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
        PersoonPartial s_persoon = new PersoonPartial();
        MatchPersoon m_persoon = new MatchPersoon();
        public List<MatchPersoon> list_personen;

        public MatchingScore(int persoonid)
        {
            s_persoon = DAL.PersoonDal.GetPersoon(persoonid);
            dt_personen = DAL.StamboomDAL.MatchPersonen(persoonid);
        }

        public PersoonPartial S_Persoon
        {
            get
            {
                return s_persoon;
            }
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


        public void StartMatch()
        {

            using (IDataReader reader = dt_personen.CreateDataReader())
            {
                list_personen = new List<MatchPersoon>();
                while (reader.Read())
                {
                    m_persoon = new MatchPersoon();
                    m_persoon.Totaal = 0;
                    int ldistance = 0;
                    
                    //Voornaam
                    if (reader.GetValue(1) != DBNull.Value)
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(1), s_persoon.voornaam);

                        if (ldistance > 0 && ldistance < 3)
                        {
                            m_persoon.ScoreVoornaam += ldistance;                            
                        }
                        if (reader.GetValue(1).ToString() == s_persoon.voornaam)
                        {
                            m_persoon.Totaal++;
                        }
                        if (reader.GetValue(1).ToString().StartsWith(s_persoon.voornaam))
                        {
                            var test = s_persoon.voornaam;
                        }                        
                    }
                    //overigenamen
                    if (reader.GetValue(2) != DBNull.Value)
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(2), s_persoon.overigenamen);
                        if (ldistance > 0 && ldistance < 3)
                        {
                            m_persoon.ScoreOverigenamen += ldistance;
                        }
                        if (reader.GetValue(2).ToString() == s_persoon.overigenamen)
                        {
                            m_persoon.Totaal++;
                        }
                    }
                    //tussenvoegsel en achternaam
                    if (reader.GetValue(3) != DBNull.Value && reader.GetValue(4) != DBNull.Value)
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(3) + " " + reader.GetString(4), s_persoon.GeefAchternaam);
                        if (ldistance > 0 && ldistance < 3)
                        {
                            m_persoon.ScoreAchternaam += ldistance;
                        }
                        if (reader.GetValue(3).ToString() + " " + reader.GetString(4) == s_persoon.overigenamen)
                        {
                            m_persoon.Totaal++;
                        }
                    }
                    //achternaam
                    else if (reader.GetValue(4) != DBNull.Value)
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(4), s_persoon.achternaam);
                        if (ldistance > 0 && ldistance < 3)
                        {
                            m_persoon.ScoreAchternaam += ldistance;
                        }
                        if (reader.GetValue(4).ToString() == s_persoon.achternaam)
                        {
                            m_persoon.Totaal++;
                        }

                    }
                    //achtervoegsel
                    if (reader.GetValue(5) != DBNull.Value)
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(5), s_persoon.achtervoegsel);
                        if (ldistance > 0 && ldistance < 3)
                        {
                            m_persoon.ScoreAchtervoegsel += ldistance;
                        }
                        if (reader.GetValue(5).ToString() == s_persoon.achtervoegsel)
                        {
                            m_persoon.Totaal++;
                        }
                    }

                    //geboortenaam
                    if (reader.GetValue(6) != DBNull.Value && reader.GetString(6) == s_persoon.geboortenaam)
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(6), s_persoon.geboortenaam);
                        if (ldistance > 0 && ldistance < 3)
                        {
                            m_persoon.ScoreGeboortenaam += ldistance;
                        }
                        if (reader.GetValue(6).ToString() == s_persoon.geboortenaam)
                        {
                            m_persoon.Totaal++;
                        }
                    }

                    if (m_persoon.CheckLevenhstein())
                    {
                        m_persoon.persoonid = reader.GetInt32(0);
                        m_persoon.voornaam = (reader.GetValue(1) != null ? reader.GetValue(1).ToString() : string.Empty);
                        m_persoon.overigenamen = (reader.GetValue(2) != null ? reader.GetValue(2).ToString() : string.Empty);
                        m_persoon.tussenvoegsel = (reader.GetValue(3) != null ? reader.GetValue(3).ToString() : string.Empty);
                        m_persoon.achternaam = (reader.GetValue(4) != null ? reader.GetValue(4).ToString() : string.Empty);
                        m_persoon.achtervoegsel = (reader.GetValue(5) != null ? reader.GetValue(5).ToString() : string.Empty);
                        m_persoon.geboortenaam = (reader.GetValue(6) != null ? reader.GetValue(6).ToString() : string.Empty);
                        m_persoon.geslacht = (reader.GetValue(7) != null ? reader.GetValue(7).ToString() : string.Empty);
                        m_persoon.status = (reader.GetValue(8) != null ? reader.GetValue(8).ToString() : string.Empty);
                        m_persoon.geboortedatum = (reader.GetValue(9) != null ? string.Format("{0:dd-MM-yyyy}", reader.GetValue(9)) : string.Empty);
                        m_persoon.geboorteprecisie = (reader.GetValue(10) != null ? reader.GetValue(10).ToString() : string.Empty);
                        m_persoon.geboortedatum2 = (reader.GetValue(11) != null ? string.Format("{0:dd-MM-yyyy}", reader.GetValue(11)) : string.Empty);
                        m_persoon.AantalAanvullendInfo = ((int)reader.GetValue(12) > 0 ? reader.GetInt32(12) : 0);
                        list_personen.Add(m_persoon);
                    }
                }
            }
            list_personen = list_personen.OrderByDescending(x => x.Totaal).ThenByDescending(x => x.AantalAanvullendInfo).ToList();
            dt_personen.Dispose();
        }     
    }

    static class LevenshteinDistance
    {
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }

}