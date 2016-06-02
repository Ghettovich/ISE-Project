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

        /// <summary>
        /// Dient alleen ff om de personen in een dropdown te krijgen voor toevoegen relaties
        /// </summary>

        public static List<SelectListItem> PersonenLijst(int stamboomid)
        {
            List<PersoonPartial> personen = PersonenStamboom(stamboomid);
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
        
        /// <summary>
        /// Haalt de verschillende geboorteprecisies op
        /// </summary>
        /// <returns>List met selectlistitems</returns>
        public static List<SelectListItem> geboortePrecisies()
        {
            List<SelectListItem> datumPrecisies = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            SelectListItem item2 = new SelectListItem();
            SelectListItem item3 = new SelectListItem();
            SelectListItem item4 = new SelectListItem();
            item.Value = "op";
            item.Text = "op";
            datumPrecisies.Add(item);
            item2.Value = "voor";
            item2.Text = "voor";
            datumPrecisies.Add(item2);
            item3.Value = "na";
            item3.Text = "na";
            datumPrecisies.Add(item3);
            item4.Value = "tussen";
            item4.Text = "tussen";
            datumPrecisies.Add(item4);

            return datumPrecisies;

        }

        //vervangen met sp
        public static List<SelectListItem> statussen()
        {
            List<SelectListItem> statussen = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            SelectListItem item2 = new SelectListItem();
            item.Value = "0";
            item.Text = "overleden";
            statussen.Add(item);
            item2.Value = "1";
            item2.Text = "levend";
            statussen.Add(item2);

            return statussen;
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
                                    geboortedatum = (results.GetValue(9) != null ? results.GetValue(9).ToString().Substring(0, 9) : string.Empty),
                                    geboorteprecisie = (results.GetValue(10) != null ? results.GetValue(10).ToString() : string.Empty),
                                    geboortedatum2 = (results.GetValue(11) != null ? results.GetValue(11).ToString() : string.Empty),
                                    gewijzigdOp = DateTime.Parse(results.GetValue(12).ToString())
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
        public static List<PersoonPartial> PersonenStamboom(int stamboomid)
        {

            try
            {
                List<PersoonPartial> persoon_list = new List<PersoonPartial>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.spd_AllePersonenStamboom";

                        IDataParameter voornaam_dp = cmd.CreateParameter();
                        voornaam_dp.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@stamboomid", string.IsNullOrEmpty(stamboomid.ToString())
                            ? (object)DBNull.Value : stamboomid.ToString()));

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
                                    tussenvoegsel = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                    achternaam = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    geboortenaam = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    geslacht = (results.GetValue(5) != null ? results.GetValue(5).ToString() : string.Empty),
                                    status = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                    geboortedatum = (results.GetValue(7) != null ? results.GetValue(7).ToString().Substring(0, 9) : string.Empty),
                                    geboorteprecisie = (results.GetValue(8) != null ? results.GetValue(8).ToString() : string.Empty),
                                    geboortedatum2 = (results.GetValue(9) != null ? results.GetValue(9).ToString() : string.Empty),
                                    kekuleid = (int)results.GetValue(10)

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

        public static List<PersoonPartial> PersonenInStamboom(int stamboomid)
        {

            try
            {
                List<PersoonPartial> persoon_list = new List<PersoonPartial>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.spd_AllePersonenInStamboom";

                        IDataParameter voornaam_dp = cmd.CreateParameter();
                        voornaam_dp.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@stamboomid", string.IsNullOrEmpty(stamboomid.ToString())
                            ? (object)DBNull.Value : stamboomid.ToString()));

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
                                    tussenvoegsel = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                    achternaam = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    geboortenaam = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    geslacht = (results.GetValue(5) != null ? results.GetValue(5).ToString() : string.Empty),
                                    status = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                    geboortedatum = (results.GetValue(7) != null ? results.GetValue(7).ToString().Substring(0, 9) : string.Empty),
                                    geboorteprecisie = (results.GetValue(8) != null ? results.GetValue(8).ToString() : string.Empty),
                                    geboortedatum2 = (results.GetValue(9) != null ? results.GetValue(9).ToString() : string.Empty),
                                    kekuleid = (int)results.GetValue(10)

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

        public static List<PersoonPartial> PersonenNietInStamboom(int stamboomid)
        {

            try
            {
                List<PersoonPartial> persoon_list = new List<PersoonPartial>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.spd_AllePersonenNietInStamboom";

                        IDataParameter voornaam_dp = cmd.CreateParameter();
                        voornaam_dp.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@stamboomid", string.IsNullOrEmpty(stamboomid.ToString())
                            ? (object)DBNull.Value : stamboomid.ToString()));

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
                                    tussenvoegsel = (results.GetValue(2) != null ? results.GetValue(2).ToString() : string.Empty),
                                    achternaam = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    geboortenaam = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    geslacht = (results.GetValue(5) != null ? results.GetValue(5).ToString() : string.Empty),
                                    status = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                    geboortedatum = (results.GetValue(7) != null ? results.GetValue(7).ToString().Substring(0, 9) : string.Empty),
                                    geboorteprecisie = (results.GetValue(8) != null ? results.GetValue(8).ToString() : string.Empty),
                                    geboortedatum2 = (results.GetValue(9) != null ? results.GetValue(9).ToString() : string.Empty),
                                    kekuleid = (int)results.GetValue(10)

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
                        cmd.CommandText = "dbo.spd_AllePersonen";

                        IDataParameter voornaam_dp = cmd.CreateParameter();
                        voornaam_dp.Direction = ParameterDirection.Input;

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
                                    geboortedatum = (results.GetValue(9) != null ? results.GetValue(9).ToString().Substring(0, 9) : string.Empty),
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
                                geboortedatum2 = (results.GetValue(7) != null ? results.GetValue(7).ToString() : string.Empty),


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

        public static PersoonPartial nieuwPersoonInDatabase(PersoonModel model)
        {
            try
            {
                PersoonPartial id = null;
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

                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            object[] results = new object[reader.FieldCount];
                            reader.GetValues(results);
                            id = new PersoonPartial
                            {
                                id = (int)results.GetValue(0)
                            };
                        }
                    }
                }
                return id;
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
                        cmd.Parameters.Add(new SqlParameter("@oudWijzigdatum", string.IsNullOrEmpty(model.gewijzigdOp.ToString("yyyy-MM-dd HH:mm:ss"))
                            ? (object)DBNull.Value : model.gewijzigdOp.ToString("yyyy-MM-dd HH:mm:ss")));

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