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
                    m_persoon.LevenshteinAfstandTT = 0;
                    m_persoon.AantalKolommen = 0;
                    m_persoon.MatchKolommen = "";
                    int ldistance = 0;
                    
                    //Voornaam
                    if (reader.GetValue(1) != DBNull.Value && !string.IsNullOrEmpty(s_persoon.voornaam))
                    {
                        
                        ldistance = LevenshteinDistance.Compute(reader.GetString(1), s_persoon.voornaam);
                        if (ldistance <= 3)
                        {
                            m_persoon.LevenshteinAfstandTT += ldistance;
                            m_persoon.AantalKolommen++;
                            m_persoon.MatchKolommen += "Voornaam ";
                            if (ldistance == 0)
                            {                                
                                m_persoon.ScoreVoornaam = 10;
                            }
                            else if (ldistance == 1)
                            {
                                m_persoon.ScoreVoornaam = 7;

                            }
                            else if (ldistance == 2)
                            {
                                m_persoon.ScoreVoornaam = 4;

                            }
                            else if (ldistance == 3)
                            {
                                m_persoon.ScoreVoornaam = 1;
                            }
                        }
                    }
                    //overigenamen
                    if (reader.GetValue(2) != DBNull.Value && !string.IsNullOrEmpty(s_persoon.voornaam))
                    {
                        
                        ldistance = LevenshteinDistance.Compute(reader.GetString(2), s_persoon.overigenamen);
                        if (ldistance <= 3)
                        {
                            m_persoon.LevenshteinAfstandTT += ldistance;
                            m_persoon.AantalKolommen++;
                            m_persoon.MatchKolommen += "Overigenamen ";
                            if (ldistance == 0)
                            {
                                m_persoon.ScoreOverigenamen = 10;
                            }
                            else if (ldistance == 1)
                            {
                                m_persoon.ScoreOverigenamen = 7;

                            }
                            else if (ldistance == 2)
                            {
                                m_persoon.ScoreOverigenamen = 4;

                            }
                            else if (ldistance == 3)
                            {
                                m_persoon.ScoreOverigenamen = 1;
                            }
                        }
                    }
                    //tussenvoegsel en achternaam
                    if (reader.GetValue(3) != DBNull.Value && reader.GetValue(4) != DBNull.Value 
                        && !string.IsNullOrEmpty(s_persoon.tussenvoegsel) && !string.IsNullOrEmpty(s_persoon.achternaam))
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(3) + " " + reader.GetString(4), s_persoon.GeefAchternaam);
                        if (ldistance <= 3)
                        {
                            m_persoon.LevenshteinAfstandTT += ldistance;
                            m_persoon.AantalKolommen++;
                            m_persoon.MatchKolommen += "Achternaam ";
                            if (ldistance == 0)
                            {
                                m_persoon.ScoreAchternaam = 10;
                            }
                            else if (ldistance == 1)
                            {
                                m_persoon.ScoreAchternaam = 7;

                            }
                            else if (ldistance == 2)
                            {
                                m_persoon.ScoreAchternaam = 4;

                            }
                            else if (ldistance == 3)
                            {
                                m_persoon.ScoreAchternaam = 1;
                            }
                        }                        
                    }
                    //alleen achternaam
                    else if (reader.GetValue(4) != DBNull.Value && !string.IsNullOrEmpty(s_persoon.achternaam))
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(4), s_persoon.achternaam);
                        if (ldistance <= 3)
                        {
                            m_persoon.LevenshteinAfstandTT += ldistance;
                            m_persoon.AantalKolommen++;
                            m_persoon.MatchKolommen += "Achternaam ";
                            if (ldistance == 0)
                            {
                                m_persoon.ScoreAchternaam = 10;
                            }
                            else if (ldistance == 1)
                            {
                                m_persoon.ScoreAchternaam = 7;

                            }
                            else if (ldistance == 2)
                            {
                                m_persoon.ScoreAchternaam = 4;

                            }
                            else if (ldistance == 3)
                            {
                                m_persoon.ScoreAchternaam = 1;
                            }
                        }

                    }
                    //achtervoegsel
                    if (reader.GetValue(5) != DBNull.Value && !string.IsNullOrEmpty(s_persoon.achtervoegsel))
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(5), s_persoon.achtervoegsel);
                        if (ldistance <= 3)
                        {
                            m_persoon.LevenshteinAfstandTT += ldistance;
                            m_persoon.AantalKolommen++;
                            m_persoon.MatchKolommen += "Achtervoegsel ";
                            if (ldistance == 0)
                            {
                                m_persoon.ScoreAchtervoegsel = 10;
                            }
                            else if (ldistance == 1)
                            {
                                m_persoon.ScoreAchtervoegsel = 7;

                            }
                            else if (ldistance == 2)
                            {
                                m_persoon.ScoreAchtervoegsel = 4;

                            }
                            else if (ldistance == 3)
                            {
                                m_persoon.ScoreAchtervoegsel = 1;
                            }
                        }
                    }

                    //geboortenaam
                    if (reader.GetValue(6) != DBNull.Value && !string.IsNullOrEmpty(s_persoon.geboortenaam))
                    {
                        ldistance = LevenshteinDistance.Compute(reader.GetString(6), s_persoon.geboortenaam);
                        if (ldistance <= 3)
                        {
                            m_persoon.LevenshteinAfstandTT += ldistance;
                            m_persoon.AantalKolommen++;
                            m_persoon.MatchKolommen += "Geboortenaam ";
                            if (ldistance == 0)
                            {
                                m_persoon.ScoreGeboortenaam = 10;
                            }
                            else if (ldistance == 1)
                            {
                                m_persoon.ScoreGeboortenaam = 7;

                            }
                            else if (ldistance == 2)
                            {
                                m_persoon.ScoreGeboortenaam = 4;

                            }
                            else if (ldistance == 3)
                            {
                                m_persoon.ScoreGeboortenaam = 1;
                            }
                        }
                    }

                    if (m_persoon.MatchGevonden)
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
            list_personen = list_personen.OrderByDescending(x => x.TotaalScore).ThenByDescending(x => x.AantalKolommen).ThenByDescending(x => x.AantalAanvullendInfo).ToList();
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