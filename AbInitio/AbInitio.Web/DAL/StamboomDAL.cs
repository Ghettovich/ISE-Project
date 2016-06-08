using AbInitio.Web.App_Start;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DAL
{
    public class StamboomDAL
    {
        private static string SP_MatchPersoon = "SP_MatchPersoon";
        private const string con = "Server=localhost; Database=AbInitio; Trusted_Connection=True";

        public IDataReader reader { get; set; }

        /// <summary>
        /// Alleen voor beheerder, geeft alle stambomen terug
        /// </summary>
        /// <returns>Lijst met alle stambomen</returns>
        public static List<stamboom> Stambomen()
        {
            List<stamboom> stambomen = new List<stamboom>();
            using (DataConfig dbdc = new DataConfig())
            {
                using (IDbCommand cmd = dbdc.CreateCommand())
                {
                    cmd.CommandText = "SELECT s.stamboomid, s.familienaam, s.levensverwachtingman, s.levensverwachtingvrouw, s.langstlevendeman, s.langstlevendevrouw ";
                    cmd.CommandText += "FROM dbo.stamboom s ";

                    dbdc.Open();
                    using (IDataReader dr = dbdc.CreateSqlReader())
                    {
                        object[] results = new object[dr.FieldCount];
                        while (dr.Read())
                        {
                            dr.GetValues(results);
                            stambomen.Add(new stamboom
                            {
                                stamboomid = (int)results.GetValue(0),
                                familienaam = (results.GetValue(1) != null ? results.GetValue(1).ToString() : string.Empty),
                                levensverwachtingman = (results.GetValue(2).ToString() != string.Empty ? (int)results.GetValue(2) : 0),
                                levensverwachtingvrouw = (results.GetValue(3).ToString() != string.Empty ? (int)results.GetValue(3) : 0),
                                langstlevendeman = (results.GetValue(4).ToString() != string.Empty ? (int)results.GetValue(4) : 0),
                                langstlevendevrouw = (results.GetValue(5).ToString() != string.Empty ? (int)results.GetValue(5) : 0)
                            });
                        }
                    }
                }
            } return stambomen;
        }
        
        /// <summary>
        /// Methode voor het matchen van een persoon met andere personen in de Persoon tabel.
        /// </summary>
        /// <param name="persoonid"></param>
        /// <returns>DataTabel met personen</returns>
        public static DataTable MatchPersonen(int persoonid)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection("Server=localhost; Database=AbInitio; Trusted_Connection=True"))
            {
                using (SqlCommand cmd = new SqlCommand(SP_MatchPersoon))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@persoonid", persoonid));
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            } return dt;
        }

        /// <summary>
        /// Methode om een specefieke stamboom op te halen uit de database.
        /// </summary>
        /// <param name="stamboomid"></param>
        /// <returns>Een stamboom</returns>
        public static stamboom GetStamboom(int stamboomid)
        {
            stamboom stam = null;
            using (DataConfig dbdc = new DataConfig())
            {
                dbdc.Open();
                using (IDbCommand cmd = dbdc.CreateCommand())
                {
                    cmd.CommandText = "dbo.getStamboom";
                    cmd.CommandType = CommandType.StoredProcedure;

                    IDataParameter pm = cmd.CreateParameter();
                    pm.Direction = ParameterDirection.Input;

                    pm.ParameterName = "@stamboomid";
                    pm.Value = stamboomid;
                    cmd.Parameters.Add(pm);

                    using (IDataReader dr = dbdc.CreateSqlReader())
                    {
                        object[] results = new object[dr.FieldCount];
                        while (dr.Read())
                        {
                            dr.GetValues(results);
                            stam = new stamboom {
                                stamboomid = (int)results.GetValue(0),
                                familienaam = (results.GetValue(1) != null ? results.GetValue(1).ToString() : string.Empty),
                                levensverwachtingman = (results.GetValue(2).ToString() != string.Empty ? (int)results.GetValue(2) : 0),
                                levensverwachtingvrouw = (results.GetValue(3).ToString() != string.Empty ? (int)results.GetValue(3) : 0),
                                langstlevendeman = (results.GetValue(4).ToString() != string.Empty ? (int)results.GetValue(4) : 0),
                                langstlevendevrouw = (results.GetValue(5).ToString() != string.Empty ? (int)results.GetValue(5) : 0),
                                jongstlevendeman = (results.GetValue(6).ToString() != string.Empty ? (int)results.GetValue(6) : 0),
                                jongstlevendevrouw = (results.GetValue(7).ToString() != string.Empty ? (int)results.GetValue(7) : 0),
                                gemiddeldaantalkinderen = (results.GetValue(8).ToString() != string.Empty ? (int)results.GetValue(8) : 0),
                                gemiddeldaantalgeboortes = (results.GetValue(9).ToString() != string.Empty ? (int)results.GetValue(9) : 0),
                                afgeschermd = Convert.ToBoolean(results.GetValue(10)),
                                gewijzigdOp = DateTime.Parse(results.GetValue(11).ToString())

                            };
                        }
                    }
                }
            } return stam;
        }

        /// <summary>
        /// Methode voor het aanmaken van een stamboom in de database.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="familieNaam"></param>
        /// <returns>Een stamboom</returns>
        public stamboom maakStamboom(int accountId, string familieNaam)
        {
                stamboom stam = null;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.aanmakenStamboom";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@accountId";
                        pm.Value = accountId;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@familienaam";
                        pm.Value = familieNaam;
                        cmd.Parameters.Add(pm);

                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        object[] results = new object[reader.FieldCount];
                        reader.GetValues(results);
                            stam = new stamboom
                            {
                                stamboomid = (int)results.GetValue(0)
                            };
                         }
                        }
                    }
                return stam;
            } 
        

        /// <summary>
        /// Een methode om stambomen op te zoeken met een bepaalde familienaam.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="familieNaam"></param>
        /// <returns>Lijst van stambomen met dezelfde familienaam</returns>
        public List<StamboomModel> getStambomen(int accountId, string familieNaam)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.zoekenStambomen";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@opvrager";
                        pm.Value = accountId;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@familienaam";
                        pm.Value = "%" + familieNaam + "%";
                        cmd.Parameters.Add(pm);

                        reader = cmd.ExecuteReader();

                        List<StamboomModel> stambomen = new List<StamboomModel>();
                        StamboomModel stamboom;

                        while (reader.Read())
                        {
                            stamboom = new StamboomModel();
                            stamboom.stamboomId = (int)reader["stamboomid"];
                            stamboom.accountId = (int)reader["accountid"];
                            stamboom.familieNaam = reader["familienaam"].ToString();
                            stambomen.Add(stamboom);
                        }
                        return stambomen;
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Een methode voor het afschermen van een stamboom.
        /// </summary>
        /// <param name="stamboomid"></param>
        public void afschermenStamboom(int stamboomid)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_AfschermenGegevens";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@stamboomid";
                        pm.Value = stamboomid;
                        cmd.Parameters.Add(pm);

                        cmd.ExecuteReader();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Een methode voor het verwijderen van een stamboom.
        /// </summary>
        /// <param name="accountid"></param>
        /// <param name="stamboomid"></param>
        public void verwijderStamboom(int accountid, int stamboomid)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_VerwijderStamboom";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@stamboomid";
                        pm.Value = stamboomid;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@accountid";
                        pm.Value = accountid;
                        cmd.Parameters.Add(pm);

                        cmd.ExecuteReader();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Een methode voor het toevoegen van een persoon in de persoon tabel en in de personeninstamboom tabel.
        /// </summary>
        /// <param name="stamboomid"></param>
        /// <param name="persoonid"></param>
        public static void persoonInStamboom(int stamboomid,int persoonid)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.personenToevoegenInStamboom";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;
                        pm.ParameterName = "@stamboomid";
                        pm.Value = stamboomid;
                        cmd.Parameters.Add(pm);

                        IDataParameter pm2 = cmd.CreateParameter();
                        pm2.Direction = ParameterDirection.Input;
                        pm2.ParameterName = "@persoonid";
                        pm2.Value = persoonid;
                        cmd.Parameters.Add(pm2);


                        cmd.ExecuteReader();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Een methode om de naam van een stamboom te wijzigen
        /// </summary>
        /// <param name="model"></param>
        public void wijzigStamboom(StamboomModel model)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_WijzigStamboom";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@stamboomid", model.stamboomId));
                        cmd.Parameters.Add(new SqlParameter("@familienaam", string.IsNullOrEmpty(model.familieNaam)
                        ? (object)DBNull.Value : model.familieNaam));
                        cmd.Parameters.Add(new SqlParameter("@oudWijzigdatum", string.IsNullOrEmpty(model.gewijzigdOp.ToString("yyyy-MM-dd HH:mm:ss"))
                        ? (object)DBNull.Value : model.gewijzigdOp.ToString("yyyy-MM-dd HH:mm:ss")));

                        cmd.ExecuteReader();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Methode om alle stambomen op te halen waar een account de eigenaar van is.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Lijst van stambomen</returns>
        public List<StamboomModel> getEigenStambomen(int accountId)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.zoekenEigenStambomen";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@opvrager";
                        pm.Value = accountId;
                        cmd.Parameters.Add(pm);

                        reader = cmd.ExecuteReader();

                        List<StamboomModel> stambomen = new List<StamboomModel>();
                        StamboomModel stamboom;

                        while (reader.Read())
                        {
                            stamboom = new StamboomModel();
                            stamboom.stamboomId = (int)reader["stamboomid"];
                            stamboom.accountId = (int)reader["accountid"];
                            stamboom.familieNaam = reader["familienaam"].ToString();
                            stambomen.Add(stamboom);
                        }
                        return stambomen;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Methode om alle stambomen op te halen waar een account als collaborateur staat.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Lijst van stambomen</returns>
        public List<StamboomModel> getCollaboratieStambomen(int accountId)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.zoekenCollaboratieStambomen";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@opvrager";
                        pm.Value = accountId;
                        cmd.Parameters.Add(pm);

                        reader = cmd.ExecuteReader();

                        List<StamboomModel> stambomen = new List<StamboomModel>();
                        StamboomModel stamboom;

                        while (reader.Read())
                        {
                            stamboom = new StamboomModel();
                            stamboom.stamboomId = (int)reader["stamboomid"];
                            stamboom.accountId = (int)reader["accountid"];
                            stamboom.familieNaam = reader["familienaam"].ToString();
                            stambomen.Add(stamboom);
                        }
                        return stambomen;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Methode om alle personen op te halen van een stamboom die in de tabel personeninstamboom staan.
        /// </summary>
        /// <param name="stamboomId"></param>
        /// <param name="aanvragerId"></param>
        /// <returns>Lijst van personen in een stamboom</returns>
        public List<PersoonInStamboom> getPersonenInStamboom(int stamboomId, int aanvragerId)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {

                        cmd.CommandText = "dbo.ophalenStamboom";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@opvrager";
                        pm.Value = aanvragerId;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@stamboomid";
                        pm.Value = stamboomId;
                        cmd.Parameters.Add(pm);

                        reader = cmd.ExecuteReader();

                        List<PersoonInStamboom> personeninstamboom = new List<PersoonInStamboom>();
                        PersoonInStamboom persooninstamboom;

                        while (reader.Read())
                        {
                            persooninstamboom = new PersoonInStamboom();
                            persooninstamboom.persoonInStamboomId = reader["persooninstamboomid"].ToString();
                            persooninstamboom.familieNaam = reader["familieNaam"].ToString();
                            persooninstamboom.persoonId = reader["persoonid"].ToString();
                            persooninstamboom.kekuleId = reader["kekulenummer"].ToString();
                            persooninstamboom.voornaam = reader["voornaam"].ToString();
                            persooninstamboom.tussenvoegsel = reader["tussenvoegsel"].ToString();
                            persooninstamboom.achternaam = reader["achternaam"].ToString();
                            persooninstamboom.geboortePrecisie = reader["geboorteprecisie"].ToString();
                            if (Convert.IsDBNull(reader["geboortedatum"]))
                            {
                                persooninstamboom.geboortedatum = null;
                            }
                            else
                            {
                                persooninstamboom.geboortedatum = (DateTime)reader["geboortedatum"];
                            }
                            if (Convert.IsDBNull(reader["geboortedatum2"]))
                            {
                                persooninstamboom.geboortedatum2 = null;
                            }
                            else
                            {
                                persooninstamboom.geboortedatum2 = (DateTime)reader["geboortedatum2"];
                            }
                            persooninstamboom.trouwdatum = reader["trouwdatum"].ToString();
                            persooninstamboom.overlijdingsdatum = reader["overlijdingsdatum"].ToString();
                            personeninstamboom.Add(persooninstamboom);
                        }

                        return personeninstamboom;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}