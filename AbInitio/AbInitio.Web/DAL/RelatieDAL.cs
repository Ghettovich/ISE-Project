using AbInitio.Web.App_Start;
using AbInitio.Web.DbContexts;
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
    public class RelatieDAL
    {
        private static string SP_GeefRelatieTypes = "SP_GeefRelatieTypes";
        private static string SP_AvrTypes = "SP_AvrTypes";
        private static string SP_RelatiesTotPersoon = "SP_RelatiesTotPersoon";
        private static string SP_PersonenInRelatie = "SP_PersonenInRelatie";
        private static string SP_AanvullendeRelatieInfo = "SP_AanvullendeRelatieInfo";

        private static string SP_ToevoegenAvr = "SP_ToevoegenAvr";

        private static string SP_VerwijderRelatie = "SP_VerwijderRelatie";
        private static string SP_WijzigRelatie = "SP_WijzigRelatie";
        private static string SP_ToevoegenRelatie = "SP_ToevoegenRelatie";


        //SelectLists
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
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = SP_AvrTypes;
                        //cmd.CommandText = "SELECT rit.relatieinformatietypeid, rit.relatieinformatietype FROM relatieinformatietype rit";
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
                }
                return avr;
            }
            catch (Exception)
            {
                throw;
            }
        }
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
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = SP_GeefRelatieTypes;
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            SelectListItem item;
                            while (reader.Read())
                            {
                                object[] result = new object[reader.FieldCount];
                                reader.GetValues(result);
                                item = new SelectListItem();

                                item.Value = result.GetValue(0).ToString();
                                item.Text = result.GetValue(1).ToString();

                                if (relatietypeid > 0 && relatietypeid == (int)result.GetValue(0))
                                {
                                    item.Selected = true;
                                }
                                else
                                {
                                    item.Selected = false;
                                }
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

        //Lists
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
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = SP_RelatiesTotPersoon;

                        IDataParameter dp_persoonid1;
                        dp_persoonid1 = cmd.CreateParameter();
                        dp_persoonid1.ParameterName = "@persoonid1";
                        dp_persoonid1.Value = persoonid;
                        cmd.Parameters.Add(dp_persoonid1);

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
                                    geboortedatum = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    RelatieID = (int)results.GetValue(5),
                                    RelatieType = results.GetValue(6).ToString()
                                });
                            }
                        }
                    }
                } return persoonrelaties;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<AVRelatiePartial> AanvullendeRelatieInfo(int relatieid)
        {
            try
            {
                List<AVRelatiePartial> avr = new List<AVRelatiePartial>();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = SP_AanvullendeRelatieInfo;

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
                                reader.GetValues(results);
                                avr.Add(new AVRelatiePartial
                                {
                                    aanvullenderelatieinformatieid = (int)reader.GetValue(0),
                                    AVInfoType = reader.GetValue(1).ToString(),
                                    relatieinformatie = reader.GetValue(2).ToString(),
                                    datumVan = (reader.GetValue(3) != null ? string.Format("{0:dd-MM-yyyy}", results.GetValue(3)) : string.Empty),
                                    datumPrecisie = (reader.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    datumTot = (reader.GetValue(5) != null ? string.Format("{0:dd-MM-yyyy}", results.GetValue(3)) : string.Empty),
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
        public static RelatiePartial GetRelatieInfo(int relatieid)
        {
            try
            {
                RelatiePartial rp = new RelatiePartial();
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = SP_PersonenInRelatie;

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

                        SqlParameter pm = new SqlParameter();
                        pm.ParameterName = "@relatietype";
                        pm.SqlDbType = SqlDbType.VarChar;
                        pm.Direction = ParameterDirection.Output;
                        pm.Size = 50;
                        cmd.Parameters.Add(pm);

                        IDataParameter gewijzigdOp_dp = cmd.CreateParameter();
                        gewijzigdOp_dp.Direction = ParameterDirection.Output;
                        gewijzigdOp_dp.ParameterName = "@gewijzigdOp";
                        gewijzigdOp_dp.DbType = DbType.DateTime;
                        cmd.Parameters.Add(gewijzigdOp_dp);

                        dbdc.Open();
                        cmd.ExecuteNonQuery();

                        rp.relatieid = (int)relatieid_dp.Value;
                        rp.persoonid1 = (int)persoonid1_dp.Value;
                        rp.persoonid2 = (int)persoonid2_dp.Value;
                        rp.relatietypeid = (int)relatietypeid_dp.Value;
                        rp.RelatieType = pm.Value.ToString();
                        rp.gewijzigdOp = (DateTime)gewijzigdOp_dp.Value;
                    }
                }
                return rp;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Voids
        public static void VerwijderRelatie(int relatieid, out string error)
        {
            try
            {
                error = string.Empty;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = SP_VerwijderRelatie;
                        cmd.CommandType = CommandType.StoredProcedure;
                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;
                        pm.ParameterName = "@relatieid";
                        pm.Value = relatieid;
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
                        cmd.CommandText = SP_WijzigRelatie;
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
                        cmd.CommandText = SP_ToevoegenRelatie;
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


    }
}