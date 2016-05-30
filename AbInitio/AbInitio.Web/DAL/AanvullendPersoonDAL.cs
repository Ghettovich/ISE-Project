using AbInitio.Web.App_Start;
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
    public class AanvullendPersoonDAL
    {
        public static List<SelectListItem> datumPrecisiesOphalen()
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

        public static List<SelectListItem> aanvullendPersoonInformatieTypesOphalen()
        {
            try
            {
                List<SelectListItem> aanvullendPersoonInformatieTypes = new List<SelectListItem>();
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_AanvullendPersoonInformatieTypes";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (IDataReader reader = dbdc.CreateSqlReader())
                        {
                            while (reader.Read())
                            {
                                object[] test = new object[reader.FieldCount];
                                reader.GetValues(test);
                                SelectListItem item = new SelectListItem();

                                item.Value = test.GetValue(0).ToString();
                                item.Text = test.GetValue(1).ToString();
                                aanvullendPersoonInformatieTypes.Add(item);
                            }
                        }
                    }
                }
                return aanvullendPersoonInformatieTypes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static void nieuwAanvullendPersoonInDatabase(AanvullendPersoonModel model)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_MaakAanvullendPersoon";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@persoonid", model.persoonid));
                        cmd.Parameters.Add(new SqlParameter("@persooninformatietypeid", model.persooninformatietypeid));
                        cmd.Parameters.Add(new SqlParameter("@persooninformatie", string.IsNullOrEmpty(model.persooninformatie)
                            ? (object)DBNull.Value : model.persooninformatie));
                        cmd.Parameters.Add(new SqlParameter("@van", string.IsNullOrEmpty(model.van)
                            ? (object)DBNull.Value : model.van));
                        cmd.Parameters.Add(new SqlParameter("@tot", string.IsNullOrEmpty(model.tot)
                            ? (object)DBNull.Value : model.tot));
                        cmd.Parameters.Add(new SqlParameter("@datumPrecisie", string.IsNullOrEmpty(model.datumPrecisie)
                            ? (object)DBNull.Value : model.datumPrecisie));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static AanvullendPersoonModel getAanvullendPersoon(int id)
        {
            try
            {
                AanvullendPersoonModel aanvullendPersoon = null;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_SelecteerAanvullendPersoon";
                        cmd.CommandType = CommandType.StoredProcedure;
                        IDataParameter dp = cmd.CreateParameter();
                        dp.ParameterName = "@aanvullendPersoonInformatieId";
                        dp.Value = id;
                        cmd.Parameters.Add(dp);

                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            object[] results = new object[dr.FieldCount];
                            while (dr.Read())
                            {
                                dr.GetValues(results);
                                aanvullendPersoon = new AanvullendPersoonModel
                                {
                                    aanvullendepersooninformatieid = (int)results.GetValue(0),
                                    persoonid = (int)results.GetValue(1),
                                    persooninformatietypeid = (int)results.GetValue(2),
                                    persooninformatie = (results.GetValue(3) != null ? results.GetValue(3).ToString() : string.Empty),
                                    van = (results.GetValue(4) != null ? results.GetValue(4).ToString() : string.Empty),
                                    tot = (results.GetValue(5) != null ? results.GetValue(5).ToString() : string.Empty),
                                    datumPrecisie = (results.GetValue(6) != null ? results.GetValue(6).ToString() : string.Empty),
                                    gewijzigdOp = (DateTime.Parse(results.GetValue(7).ToString()))

                                };
                            }
                        }
                    }
                }
                return aanvullendPersoon;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void wijzigAanvullendPersoonInDatabase(AanvullendPersoonModel model)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_VeranderAanvullendPersoon";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@aanvullendePersoonInformatieId", model.aanvullendepersooninformatieid));
                        cmd.Parameters.Add(new SqlParameter("@persooninformatietypeid", model.persooninformatietypeid));
                        cmd.Parameters.Add(new SqlParameter("@persooninformatie", string.IsNullOrEmpty(model.persooninformatie)
                            ? (object)DBNull.Value : model.persooninformatie));
                        cmd.Parameters.Add(new SqlParameter("@van", string.IsNullOrEmpty(model.van)
                            ? (object)DBNull.Value : model.van));
                        cmd.Parameters.Add(new SqlParameter("@tot", string.IsNullOrEmpty(model.tot)
                            ? (object)DBNull.Value : model.tot));
                        cmd.Parameters.Add(new SqlParameter("@datumPrecisie", string.IsNullOrEmpty(model.datumPrecisie)
                            ? (object)DBNull.Value : model.datumPrecisie));
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

        public static void verwijderAanvullendPersoonInDatabase(int id)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_VerwijderAanvullendPersoon";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        cmd.Parameters.Add(new SqlParameter("@aanvullendPersoonInformatieId", id));

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