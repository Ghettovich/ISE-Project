using AbInitio.Web.App_Start;
using AbInitio.Web.DbContexts;
using AbInitio.Web.ViewModels;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace AbInitio.Web.DAL
{
    public class PersoonDal
    {
        private static IDataReader reader;

        //vervangen met sp
        /// <summary>
        /// Selectlist kun je gemakkelijk een dropdown meemaken op de view, 
        /// je zet er de Value (e.g. relatietypeid) zodat deze weer met de view kan worden meegestuurd
        /// Text is voor de items die in de dropdown te zien zijn
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> RelatieTypes(int relatietypeid)
        {
            try
            {
                List<SelectListItem> relaties = new List<SelectListItem>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT relatietypeid, relatietype FROM relatietype";
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            while (reader.Read())
                            {
                                object[] test = new object[reader.FieldCount];
                                reader.GetValues(test);
                                SelectListItem item = new SelectListItem();
                                
                                item.Value = test.GetValue(0).ToString();
                                item.Text = test.GetValue(1).ToString();
                                relaties.Add(item);

                                if (relatietypeid > 0 && relatietypeid == (int)test.GetValue(0))
                                {
                                    item.Selected = true;
                                }
                                else
                                {
                                    item.Selected = false;
                                }
                            }
                        }
                    }
                }
                return relaties;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Dient alleen ff om de personen in een dropdown te krijgen voor toevoegen relaties
        /// </summary>

        public static List<SelectListItem> PersonenLijst(int stamboomid)
        {
            List<PersoonPartial> personen = PersonenInStamboom(stamboomid);
            List<SelectListItem> listitems = new List<SelectListItem>();

            foreach (var item in personen)
            {
                SelectListItem listitem = new SelectListItem();
                listitem.Selected = false;
                listitem.Text = item.GeefVolledigeNaam;
                listitem.Value = item.persoonid.ToString();
                listitems.Add(listitem);
            } return listitems;
        }
        
        //vervangen met sp
        public static List<SelectListItem> AvrTypes()
        {
            try
            {
                List<SelectListItem> avr = new List<SelectListItem>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT rit.relatieinformatietypeid, rit.relatieinformatietype FROM relatieinformatietype rit";
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            while (reader.Read())
                            {
                                object[] test = new object[reader.FieldCount];
                                reader.GetValues(test);
                                SelectListItem item = new SelectListItem();
                                item.Selected = false;
                                item.Value = test.GetValue(0).ToString();
                                item.Text = test.GetValue(1).ToString();
                                avr.Add(item);
                            }
                        }
                    }
                } return avr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Haalt de verschillende geboorteprecisies op
        /// </summary>
        /// <returns>List met selectlistitems</returns>
        public static List<SelectListItem> geboortePrecisies()
        {
            try
            {
                List<SelectListItem> geboortePrecisies = new List<SelectListItem>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT DISTINCT geboorteprecisie FROM persoon WHERE geboorteprecisie IS NOT NULL";
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            while (reader.Read())
                            {
                                object[] test = new object[reader.FieldCount];
                                reader.GetValues(test);
                                SelectListItem item = new SelectListItem();
                                item.Selected = false;
                                item.Value = test.GetValue(0).ToString();
                                item.Text = test.GetValue(0).ToString();
                                geboortePrecisies.Add(item);
                            }
                        }
                    }
                }
                return geboortePrecisies;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //vervangen met sp
        public static List<SelectListItem> statussen()
        {
            try
            {
                List<SelectListItem> geboortePrecisies = new List<SelectListItem>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT DISTINCT status FROM persoon WHERE status IS NOT NULL ORDER BY status";
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            while (reader.Read())
                            {
                                object[] test = new object[reader.FieldCount];
                                reader.GetValues(test);
                                SelectListItem item = new SelectListItem();
                                item.Selected = false;
                                item.Value = test.GetValue(0).ToString();
                                if (item.Value.ToString().Equals("False"))
                                {
                                    item.Text = "Overleden";
                                }
                                else
                                {
                                    item.Text = "Levend";
                                }

                                geboortePrecisies.Add(item);
                            }
                        }
                    }
                }
                return geboortePrecisies;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<SelectListItem> geslachtOptiesOphalen()
        {
            List<SelectListItem> geslachtOpties = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            SelectListItem item2 = new SelectListItem();
            item.Selected = false;
            item.Value = "M";
            item.Text = "Man";
            geslachtOpties.Add(item);
            item2.Value = "V";
            item2.Text = "Vrouw";
            geslachtOpties.Add(item2);

            return geslachtOpties;
        }

        //vervangen met sp
        /// <summary>
        /// Haalt de personen op en de relatie types tot deze persoon
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<PersoonPartial> RelatiesTotPersoon(int persoonid)
        {
            try
            {
                List<PersoonPartial> persoonrelaties = new List<PersoonPartial>();

                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT p.persoonid, p.voornaam, p.tussenvoegsel, p.achternaam, r.relatieid, rt.relatietype ";
                        cmd.CommandText += "FROM persoon p INNER JOIN relatie r ON p.persoonid = r.persoonid2 ";
                        cmd.CommandText += "INNER JOIN relatietype rt ON r.relatietypeid = rt.relatietypeid ";
                        cmd.CommandText += "WHERE r.persoonid1 = @persoonid;";

                        IDataParameter dp;
                        dp = cmd.CreateParameter();
                        dp.ParameterName = "@persoonid";
                        dp.Value = persoonid;
                        cmd.Parameters.Add(dp);

                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            object[] results = new object[dr.FieldCount];

                            while (dr.Read())
                            {
                                dr.GetValues(results);
                                persoonrelaties.Add(new PersoonPartial
                                {
                                    persoonid = (int)results.GetValue(0),
                                    voornaam = (results.GetValue(1) != null ? results.GetValue(1).ToString() : string.Empty),
                                    tussenvoegsel = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                    achternaam = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    RelatieID = (int)results.GetValue(4),
                                    RelatieType = results.GetValue(5).ToString()
                                });
                            }
                        }
                    }
                }
                return persoonrelaties;
            }
            catch (Exception)
            {

                throw;
            }


        }
        
        //vervangen met sp
        /// <summary>
        /// Vind een persoon in de persoon tabel middels de id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>persoon</returns>
        public static PersoonPartial GetPersoon(int id)
        {
            try
            {
                PersoonPartial prson = null;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM persoon WHERE persoonid = @persoonid";
                        IDataParameter dp = cmd.CreateParameter();
                        dp.ParameterName = "@persoonid";
                        dp.Value = id;
                        cmd.Parameters.Add(dp);

                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            object[] results = new object[dr.FieldCount];
                            while (dr.Read())
                            {
                                dr.GetValues(results);
                                prson = new PersoonPartial
                                {
                                    persoonid = (int)results.GetValue(0),
                                    voornaam = (results.GetValue(1) != null ? results.GetValue(1).ToString() : string.Empty),
                                    overigenamen = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                    tussenvoegsel = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    achternaam = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    achtervoegsel = (results.GetValue(5) != null ? results.GetValue(5).ToString() : string.Empty),
                                    geboortenaam = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                    geslacht = (results.GetValue(7) != null ? results.GetValue(7).ToString() : string.Empty),
                                    status = (results.GetValue(8) != null ? results.GetValue(8).ToString() : string.Empty),
                                    geboortedatum = (results.GetValue(9) != null ? results.GetValue(9).ToString() : string.Empty),
                                    geboorteprecisie = (results.GetValue(10) != null ? results.GetValue(10).ToString() : string.Empty),
                                    geboortedatum2 = (results.GetValue(11) != null ? results.GetValue(11).ToString() : string.Empty)
                                };
                            }
                        }
                    }
                }
                return prson;
            }
            catch (Exception)
            {

                throw;
            }


        }

        //vervangen met sp
        /// <summary>
        /// Geeft alle stambomen van een account
        /// </summary>
        /// <param name="accountid"></param>
        /// <returns>Lijst met stambomen waarin de account id toegang heeft</returns>
        public static List<stamboom> GebruikerStambomen(int accountid)
        {
            try
            {
                List<stamboom> stambomen = new List<stamboom>();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT s.stamboomid, s.familienaam FROM dbo.stamboom s INNER JOIN dbo.stamboomtoegang stg ON stg.stamboomid = s.stamboomid AND stg.stamboomaccountid = @accountid";
                        cmd.CommandType = CommandType.Text;
                        IDbDataParameter dp;
                        dp = cmd.CreateParameter();
                        dp.ParameterName = "@accountid";
                        dp.Value = accountid;
                        cmd.Parameters.Add(dp);

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
                                    familienaam = results.GetValue(1).ToString()
                                });
                            }
                        }
                    }
                }
                return stambomen;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //vervangen met sp
        /// <summary>
        /// Geeft alle stambomen waarin de persoon zich bevind
        /// </summary>
        /// <param name="persoonid"></param>
        /// <returns>Lijst met stambomen waar in de persoon voorkomt</returns>
        public static List<stamboom> PersoonInStambomen(int persoonid)
        {

            try
            {
                List<stamboom> stambomen = new List<stamboom>();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT s.stamboomid, s.familienaam FROM dbo.stamboom s ";
                        cmd.CommandText += "INNER JOIN dbo.personeninstamboom pis ON pis.stamboomid = s.stamboomid AND pis.persoonid = @persoonid";
                        cmd.CommandType = CommandType.Text;
                        IDbDataParameter dp;
                        dp = cmd.CreateParameter();
                        dp.ParameterName = "@persoonid";
                        dp.Value = persoonid;
                        cmd.Parameters.Add(dp);

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
                                    familienaam = results.GetValue(1).ToString()
                                });
                            }
                        }
                    }
                }
                return stambomen;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //vervangen met sp
        /// <summary>
        /// Geeft alle personen van in stamboom
        /// </summary>
        /// <param name="stamboomid"></param>
        /// <returns></returns>
        public static List<PersoonPartial> PersonenInStamboom(int stamboomid)
        {

            try
            {
                List<PersoonPartial> personen = new List<PersoonPartial>();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT p.persoonid, p.voornaam, p.overigenamen, p.tussenvoegsel, p.achternaam, p.achtervoegsel, ";
                        cmd.CommandText += "p.geboortenaam, p.geslacht, p.status, p.geboortedatum, p.geboorteprecisie ";
                        cmd.CommandText += "FROM dbo.persoon p ";
                        cmd.CommandText += "INNER JOIN dbo.personeninstamboom pis ON pis.persoonid = p.persoonid ";
                        cmd.CommandText += "WHERE pis.stamboomid = @stamboomid";

                        IDbDataParameter dp = cmd.CreateParameter();
                        dp.ParameterName = "@stamboomid";
                        dp.Value = stamboomid;
                        cmd.Parameters.Add(dp);

                        dbdc.Open();
                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            object[] results = new object[dr.FieldCount];
                            while (dr.Read())
                            {

                                dr.GetValues(results);
                                personen.Add(new PersoonPartial
                                {
                                    persoonid = (int)results.GetValue(0),
                                    voornaam = (results.GetValue(1) != null ? results.GetValue(1).ToString() : string.Empty),
                                    overigenamen = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                    tussenvoegsel = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    achternaam = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    achtervoegsel = (results.GetValue(5) != null ? results.GetValue(5).ToString() : string.Empty),
                                    geboortenaam = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                    geslacht = (results.GetValue(7) != null ? results.GetValue(7).ToString() : string.Empty),
                                    status = (results.GetValue(8) != null ? results.GetValue(8).ToString() : string.Empty),
                                    geboortedatum = (results.GetValue(9) != null ? results.GetValue(9).ToString() : string.Empty),
                                    geboorteprecisie = (results.GetValue(10) != null ? results.GetValue(10).ToString() : string.Empty)
                                });
                            }
                        }
                    }
                }
                return personen;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void PersonenInRelatie(int relatieid, out int persoon1, out int persoon2, out int relatietypeid)
        {
            try
            {
                persoon1 = 0;
                persoon2 = 0;
                relatietypeid = 0;
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.SP_PersonenInRelatie";
                        //cmd.CommandText = "SELECT r.persoonid1, r.persoonid2, r.relatietypeid FROM relatie r WHERE r.relatieid = @relatieid";

                        IDataParameter relatieid_dp = cmd.CreateParameter();
                        relatieid_dp.Direction = ParameterDirection.Input;
                        relatieid_dp.ParameterName = "@relatieid";
                        relatieid_dp.Value = relatieid;                        
                        cmd.Parameters.Add(relatieid_dp);

                        IDataParameter persoonid1_dp = cmd.CreateParameter();
                        persoonid1_dp.Direction = ParameterDirection.Output;
                        persoonid1_dp.ParameterName = "@persoonid1";
                        persoonid1_dp.DbType = DbType.Int32;
                        cmd.Parameters.Add(persoonid1_dp);

                        IDataParameter persoonid2_dp = cmd.CreateParameter();
                        persoonid2_dp.Direction = ParameterDirection.Output;
                        persoonid2_dp.ParameterName = "@persoonid2";
                        persoonid2_dp.DbType = DbType.Int32;
                        cmd.Parameters.Add(persoonid2_dp);

                        IDataParameter relatietypeid_dp = cmd.CreateParameter();
                        relatietypeid_dp.Direction = ParameterDirection.Output;
                        relatietypeid_dp.ParameterName = "@relatietypeid";
                        relatietypeid_dp.DbType = DbType.Int32;
                        cmd.Parameters.Add(relatietypeid_dp);

                        dbdc.Open();
                        cmd.ExecuteNonQuery();
                        persoon1 = (int)persoonid1_dp.Value;
                        persoon2 = (int)persoonid2_dp.Value;
                        relatietypeid = (int)relatietypeid_dp.Value;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //vervangen met sp
        public static RelatiePartial GetRelatieInfo(int relatieid)
        {
            try
            {
                RelatiePartial rp = new RelatiePartial();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT rt.relatietype FROM relatie r INNER JOIN relatietype rt ON r.relatietypeid = rt.relatietypeid ";
                        cmd.CommandText += "WHERE r.relatieid = @relatieid ";

                        IDataParameter dp = cmd.CreateParameter();
                        dp.ParameterName = "@relatieid";
                        dp.Value = relatieid;
                        cmd.Parameters.Add(dp);
                        dbdc.Open();
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            object[] results = new object[reader.FieldCount];
                            reader.Read();
                            rp.RelatieType = reader.GetValue(0).ToString();
                        }
                    }
                } return rp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //vervangen met sp
        public static List<RelatiePartial> AanvullendeRelatieInfo(int relatieid)
        {
            try
            {
                List<RelatiePartial> avr = new List<RelatiePartial>();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT avr.aanvullenderelatieinformatieid, rit.relatieinformatietype, avr.relatieinformatie ";
                        cmd.CommandText += "FROM aanvullenderelatieinformatie avr INNER JOIN relatieinformatietype rit ON avr.relatieinformatietypeid = rit.relatieinformatietypeid ";
                        cmd.CommandText += "WHERE avr.relatieid = @relatieid";

                        IDataParameter dp = cmd.CreateParameter();
                        dp.ParameterName = "@relatieid";
                        dp.Value = relatieid;
                        cmd.Parameters.Add(dp);

                        dbdc.Open();
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            object[] results = new object[reader.FieldCount];

                            while (reader.Read())
                            {
                                avr.Add(new RelatiePartial
                                {
                                    AvrRelatieID = (int)reader.GetValue(0),
                                    RelatieType = reader.GetValue(1).ToString(),
                                    RelatieInformatie = reader.GetValue(2).ToString()
                                });
                            }
                        }
                    } return avr;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static void WijzigRelatie(RelatieModel model, out string error)
        {
            try
            {
                error = string.Empty;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.SP_WijzigRelatie";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@relatieid";
                        pm.Value = model.relatieid;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@persoonid1";
                        pm.Value = model.persoonid1;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@persoonid2";
                        pm.Value = model.persoonid2;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@relatietypeid";
                        pm.Value = model.relatietypeid;
                        cmd.Parameters.Add(pm);
                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
        }

        public static void ToevoegenRelatie(RelatieModel model, out string error)
        {
            try
            {
                error = string.Empty;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.SP_ToevoegenRelatie";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;
                        
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@persoonid1";
                        pm.Value = model.persoonid1;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@persoonid2";
                        pm.Value = model.persoonid2;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@relatietypeid";
                        pm.Value = model.relatietypeid;
                        cmd.Parameters.Add(pm);

                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
        }

        public static void ToevoegenAvr(RelatieModel model, out string error)
        {
            try
            {
                error = string.Empty;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.SP_ToevoegenAvr";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@relatieid";
                        pm.Value = model.relatieid;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@relatieinformatietypeid";
                        pm.Value = model.AvRelatieID;
                        cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@relatieinformatie";
                        pm.Value = model.relatieinformatie;
                        cmd.Parameters.Add(pm);

                        cmd.ExecuteReader();
                    }
                }

            }
            catch (Exception e)
            {
                error = e.Message;
            }
        }

        /// <summary>
        /// Alleen voor beheer
        /// </summary>
        /// <returns>Geeft alle personen terug</returns>
        public static List<PersoonPartial> AllePersonen(string namen)
        {
            List<PersoonPartial> persoon_list = new List<PersoonPartial>();
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.AllePersonen";

                        IDataParameter voornaam_dp = cmd.CreateParameter();
                        voornaam_dp.Direction = ParameterDirection.Input;
                        voornaam_dp.ParameterName = "@voornaam";
                        voornaam_dp.Value = (string.IsNullOrEmpty(namen) ? (object)DBNull.Value : namen);
                        cmd.Parameters.Add(voornaam_dp);

                        dbdc.Open();
                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            object[] results;
                            while (dr.Read())
                            {
                                results = new object[dr.FieldCount];
                                dr.GetValues(results);
                                persoon_list.Add(new PersoonPartial
                                {
                                    persoonid = (int)results.GetValue(0),
                                    voornaam = (results.GetValue(1) != null ? results.GetValue(1).ToString() : string.Empty),
                                    overigenamen = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                    tussenvoegsel = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    achternaam = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    achtervoegsel = (results.GetValue(5) != null ? results.GetValue(5).ToString() : string.Empty),
                                    geboortenaam = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                    geslacht = (results.GetValue(7) != null ? results.GetValue(7).ToString() : string.Empty),
                                    status = (results.GetValue(8) != null ? results.GetValue(8).ToString() : string.Empty),
                                geboortedatum = (results.GetValue(9) != null ? results.GetValue(9).ToString() : string.Empty),
                                    geboorteprecisie = (results.GetValue(10) != null ? results.GetValue(10).ToString() : string.Empty),
                                geboortedatum2 = (results.GetValue(11) != null ? results.GetValue(11).ToString() : string.Empty)

                                });
                            }
                        }

                    }
                }
                return persoon_list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<PersoonPartial> zoekenPersonen(PersoonModel model)
        {
            List<PersoonPartial> persoon_list = new List<PersoonPartial>();
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.persoonZoekenInStamboom";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@voornaam", string.IsNullOrEmpty(model.voornaam)
                            ? (object)DBNull.Value : model.voornaam));

                        cmd.Parameters.Add(new SqlParameter("@achternaam", string.IsNullOrEmpty(model.achternaam)
                            ? (object)DBNull.Value : model.achternaam));

                        cmd.Parameters.Add(new SqlParameter("@geslacht", string.IsNullOrEmpty(model.geslacht)
                            ? (object)DBNull.Value : model.geslacht));

                        cmd.Parameters.Add(new SqlParameter("@geboortedatum", string.IsNullOrEmpty(model.geboortedatum)
                            ? (object)DBNull.Value : model.geboortedatum));

                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            object[] results = new object[reader.FieldCount];
                            reader.GetValues(results);
                            persoon_list.Add(new PersoonPartial
                            {
                                persoonid = (int)results.GetValue(0),
                                stamboomid = (int)results.GetValue(12),
                                voornaam = (results.GetValue(1) != null ? results.GetValue(1).ToString() : string.Empty),
                                overigenamen = (results.GetValue(10) != null ? results.GetValue(10).ToString() : string.Empty),
                                tussenvoegsel = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                achternaam = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                achtervoegsel = (results.GetValue(11) != null ? results.GetValue(11).ToString() : string.Empty),
                                geboortenaam = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                geslacht = (results.GetValue(8) != null ? results.GetValue(8).ToString() : string.Empty),
                                status = (results.GetValue(9) != null ? results.GetValue(9).ToString() : string.Empty),
                                geboortedatum = (results.GetValue(5) != null  ? results.GetValue(5).ToString().Substring(0,9) : string.Empty),
                                geboorteprecisie = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                geboortedatum2 = (results.GetValue(7) != null && results.GetValue(7).ToString().Length > 0 ? results.GetValue(7).ToString().Substring(0,9) : string.Empty),


                            });
                        }

                    }
                }
                return persoon_list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void nieuwPersoonInDatabase(PersoonModel model)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_MaakPersoon";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@voornaam", string.IsNullOrEmpty(model.voornaam)
                            ? (object)DBNull.Value : model.voornaam));
                        cmd.Parameters.Add(new SqlParameter("@tussenvoegsel", string.IsNullOrEmpty(model.tussenvoegsel)
                            ? (object)DBNull.Value : model.tussenvoegsel));
                        cmd.Parameters.Add(new SqlParameter("@achternaam", string.IsNullOrEmpty(model.achternaam)
                            ? (object)DBNull.Value : model.achternaam));
                        cmd.Parameters.Add(new SqlParameter("@achtervoegsel", string.IsNullOrEmpty(model.achtervoegsel)
                            ? (object)DBNull.Value : model.achtervoegsel));
                        cmd.Parameters.Add(new SqlParameter("@overigenamen", string.IsNullOrEmpty(model.overigenamen)
                            ? (object)DBNull.Value : model.overigenamen));
                        cmd.Parameters.Add(new SqlParameter("@geboortenaam", string.IsNullOrEmpty(model.geboortenaam)
                            ? (object)DBNull.Value : model.geboortenaam));
                        cmd.Parameters.Add(new SqlParameter("@geslacht", string.IsNullOrEmpty(model.geslacht)
                            ? (object)DBNull.Value : model.geslacht));
                        cmd.Parameters.Add(new SqlParameter("@status", string.IsNullOrEmpty(model.status)
                            ? (object)DBNull.Value : model.status));
                        cmd.Parameters.Add(new SqlParameter("@geboortedatum", string.IsNullOrEmpty(model.geboortedatum)
                            ? (object)DBNull.Value : model.geboortedatum));
                        cmd.Parameters.Add(new SqlParameter("@geboorteprecisie", string.IsNullOrEmpty(model.geboorteprecisie)
                            ? (object)DBNull.Value : model.geboorteprecisie));
                        cmd.Parameters.Add(new SqlParameter("@geboortedatum2", string.IsNullOrEmpty(model.geboortedatum2)
                            ? (object)DBNull.Value : model.geboortedatum2));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void wijzigPersoonInDatabase(PersoonModel model)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_VeranderPersoon";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@persoonid", model.persoonid));
                        cmd.Parameters.Add(new SqlParameter("@voornaam", string.IsNullOrEmpty(model.voornaam)
                            ? (object)DBNull.Value : model.voornaam));
                        cmd.Parameters.Add(new SqlParameter("@tussenvoegsel", string.IsNullOrEmpty(model.tussenvoegsel)
                            ? (object)DBNull.Value : model.tussenvoegsel));
                        cmd.Parameters.Add(new SqlParameter("@achternaam", string.IsNullOrEmpty(model.achternaam)
                            ? (object)DBNull.Value : model.achternaam));
                        cmd.Parameters.Add(new SqlParameter("@achtervoegsel", string.IsNullOrEmpty(model.achtervoegsel)
                            ? (object)DBNull.Value : model.achtervoegsel));
                        cmd.Parameters.Add(new SqlParameter("@overigenamen", string.IsNullOrEmpty(model.overigenamen)
                            ? (object)DBNull.Value : model.overigenamen));
                        cmd.Parameters.Add(new SqlParameter("@geboortenaam", string.IsNullOrEmpty(model.geboortenaam)
                            ? (object)DBNull.Value : model.geboortenaam));
                        cmd.Parameters.Add(new SqlParameter("@geslacht", string.IsNullOrEmpty(model.geslacht)
                            ? (object)DBNull.Value : model.geslacht));
                        cmd.Parameters.Add(new SqlParameter("@status", string.IsNullOrEmpty(model.status)
                            ? (object)DBNull.Value : model.status));
                        cmd.Parameters.Add(new SqlParameter("@geboortedatum", string.IsNullOrEmpty(model.geboortedatum)
                            ? (object)DBNull.Value : model.geboortedatum));
                        cmd.Parameters.Add(new SqlParameter("@geboorteprecisie", string.IsNullOrEmpty(model.geboorteprecisie)
                            ? (object)DBNull.Value : model.geboorteprecisie));
                        cmd.Parameters.Add(new SqlParameter("@geboortedatum2", string.IsNullOrEmpty(model.geboortedatum2)
                            ? (object)DBNull.Value : model.geboortedatum2));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void verwijderPersoonInDatabase(int id)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_VerwijderPersoon";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@persoonid", id));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}