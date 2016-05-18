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

namespace AbInitio.Web.DAL
{
    public class PersoonDal
    {
        private static IDataReader reader;

        /// <summary>
        /// Selectlist kun je gemakkelijk een dropdown meemaken op de view, 
        /// je zet er de Value (e.g. relatietypeid) zodat deze weer met de view kan worden meegestuurd
        /// Text is voor de items die in de dropdown te zien zijn
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> RelatieTypes()
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
                                item.Selected = false;
                                item.Value = test.GetValue(0).ToString();
                                item.Text = test.GetValue(1).ToString();
                                relaties.Add(item);
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
        public static List<SelectListItem> PersonenLijst()
        {
            List<PersoonPartial> personen = AllePersonen();
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

        public static void PersonenInRelatie(int relatieid, out int persoon1, out int persoon2)
        {
            try
            {
                persoon1 = 0;
                persoon2 = 0;
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {

                        cmd.CommandText = "SELECT r.persoonid1, r.persoonid2 FROM relatie r WHERE r.relatieid = @relatieid";
                        IDataParameter dp = cmd.CreateParameter();
                        dp.ParameterName = "@relatieid";
                        dp.Value = relatieid;
                        cmd.Parameters.Add(dp);

                        dbdc.Open();
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            object[] results = new object[reader.FieldCount];
                            reader.Read();
                            persoon1 = (int)reader.GetValue(0);
                            persoon2 = (int)reader.GetValue(1);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static RelatiePartial GetRelatieInfo(int relatieid)
        {
            try
            {
                RelatiePartial rp = new RelatiePartial();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT rt2.relatietype FROM relatie r INNER JOIN relatieinformatietype rt ON r.relatietypeid = rt.relatieinformatietypeid ";
                        cmd.CommandText += "INNER JOIN relatietype rt2 ON r.relatietypeid = rt2.relatietypeid ";
                        cmd.CommandText += "WHERE r.relatieid = @relatieid";
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

        public static List<aanvullenderelatieinformatie> AanvullendeRelatieInfo(int relatieid)
        {
            try
            {
                List<aanvullenderelatieinformatie> avr = new List<aanvullenderelatieinformatie>();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "SELECT avr.relatieid, avr.relatieinformatie ";
                        cmd.CommandText += "FROM aanvullenderelatieinformatie avr INNER JOIN relatie r ON avr.relatieid = r.relatieid ";
                        cmd.CommandText += "INNER JOIN relatietype rt ON avr.relatieinformatietypeid = rt.relatietypeid ";
                        cmd.CommandText += "WHERE avr.relatieid = @relatieid;";

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
                                avr.Add(new aanvullenderelatieinformatie
                                {
                                    relatieid = (int)reader.GetValue(0),
                                    relatieinformatie = reader.GetValue(1).ToString()
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
        public static List<PersoonPartial> AllePersonen()
        {
            List<PersoonPartial> persoon_list = new List<PersoonPartial>();
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        int limit = 0;
                        int count = 0;
                        cmd.CommandText = "SELECT * FROM persoon";
                        dbdc.Open();
                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            while (dr.Read() && count < limit)
                            {
                                object[] results = new object[dr.FieldCount];
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
                                count++;
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



        public static List<PersoonPartial> zoekenPersonen(string voornaam = null, string achternaam = null, string geslacht = null, string geboortedatum = null)
        {
            List<PersoonPartial> persoon_list = new List<PersoonPartial>();
            try
            {
                int accounttype = 1;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.persoonZoekenInStamboom";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        //cmd.Parameters.Add(new SqlParameter("@voornaam", string.IsNullOrEmpty(model.Persoon.voornaam)
                        //    ? (object)DBNull.Value : model.Persoon.voornaam));

                        //cmd.Parameters.Add(new SqlParameter("@achternaam", string.IsNullOrEmpty(model.Persoon.achternaam)
                        //    ? (object)DBNull.Value : model.Persoon.achternaam));

                        //cmd.Parameters.Add(new SqlParameter("@geslacht", string.IsNullOrEmpty(model.Persoon.geslacht)
                        //    ? (object)DBNull.Value : model.Persoon.geslacht));

                        //cmd.Parameters.Add(new SqlParameter("@geboortedatum", string.IsNullOrEmpty(model.Persoon.geboortedatum)
                        //    ? (object)DBNull.Value : model.Persoon.geboortedatum));


                       pm.ParameterName = "@voornaam";
                       pm.Value = voornaam;
                       cmd.Parameters.Add(pm);

                       pm = cmd.CreateParameter();
                       pm.ParameterName = "@achternaam";
                       pm.Value = achternaam;
                       cmd.Parameters.Add(pm);

                       pm = cmd.CreateParameter();
                       pm.ParameterName = "@geboortedatum";
                       pm.Value = geboortedatum;
                       cmd.Parameters.Add(pm);

                       pm = cmd.CreateParameter();
                       pm.ParameterName = "@geslacht";
                       pm.Value = geslacht;
                       cmd.Parameters.Add(pm);

                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@account";
                        pm.Value = accounttype;
                        cmd.Parameters.Add(pm);

                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            object[] results = new object[reader.FieldCount];
                            reader.GetValues(results);
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
                return persoon_list;
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

    }
}