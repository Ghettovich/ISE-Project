using AbInitio.Web.App_Start;
using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.DAL
{
    public class PersoonDal
    {
        public static List<SelectListItem> RelatieTypes()
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
            } return relaties;
        }

        //Ophalen persoon
        public static persoon GetPersoon(int id)
        {
            using (DataConfig dbdc = new DataConfig())
            {
                dbdc.Open();
                using (IDbCommand cmd = dbdc.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM persoon WHERE persoonid = @persoonid";

                    IDataParameter dp;
                    dp = cmd.CreateParameter();
                    dp.ParameterName = "@persoonid";
                    dp.Value = id;
                    cmd.Parameters.Add(dp);

                    using (IDataReader dr = dbdc.CreateSqlReader())
                    {
                        persoon p = new persoon();
                        //dynamic persoon = new persoon();
                        object[] test = new object[dr.FieldCount];

                        while (dr.Read())
                        {
                            dr.GetValues(test);
                            p = new PersoonPartial
                            {
                                persoonid = (int)test.GetValue(0),
                                voornaam = (test.GetValue(1) != null ? test.GetValue(1).ToString() : string.Empty),
                                overigenamen = (test.GetValue(2) != null ? test.GetValue(2).ToString() : string.Empty),
                                tussenvoegsel = (test.GetValue(3) != null ? test.GetValue(3).ToString() : string.Empty),
                                achternaam = (test.GetValue(4) != null ? test.GetValue(4).ToString() : string.Empty),
                                achtervoegsel = (test.GetValue(5) != null ? test.GetValue(1).ToString() : string.Empty),
                                geboortenaam = (test.GetValue(6) != null ? test.GetValue(2).ToString() : string.Empty),
                                geslacht = (test.GetValue(7) != null ? test.GetValue(3).ToString() : string.Empty),
                                status = (test.GetValue(8) != null ? test.GetValue(4).ToString() : string.Empty),
                                geboortedatum = (test.GetValue(9) != null ? test.GetValue(1).ToString() : string.Empty),
                                geboorteprecisie = (test.GetValue(10) != null ? test.GetValue(2).ToString() : string.Empty),
                                geboortedatum2 = (test.GetValue(11) != null ? test.GetValue(3).ToString() : string.Empty)
                            };
                        } return p;
                    }
                }
            }
        }

        public static List<SelectListItem> RelatiesTotPersoon(int id)
        {
            List<SelectListItem> personen = new List<SelectListItem>();

            using (DataConfig dbdc = new DataConfig())
            {
                dbdc.Open();
                using (IDbCommand cmd = dbdc.CreateCommand())
                {
                    cmd.CommandText = "SELECT p.voornaam, p.tussenvoegsel, p.achternaam";
                    cmd.CommandText += "FROM relatie r INNER JOIN persoon ON r.persoonid1 = p.persoonid";
                    cmd.CommandText += "WHERE r.persoonid1 = @persoonid";

                    IDataParameter dp;
                    dp = cmd.CreateParameter();
                    dp.ParameterName = "@persoonid1";
                    dp.Value = id;
                    cmd.Parameters.Add(dp);

                    using (IDataReader dr = dbdc.CreateSqlReader())
                    {
                        persoon p = new persoon();
                        //dynamic persoon = new persoon();
                        object[] test = new object[dr.FieldCount];

                        while (dr.Read())
                        {
                            dr.GetValues(test);
                            p = new PersoonPartial
                            {
                                persoonid = (int)test.GetValue(0),
                                voornaam = (test.GetValue(1) != null ? test.GetValue(1).ToString() : string.Empty),
                                overigenamen = (test.GetValue(2) != null ? test.GetValue(2).ToString() : string.Empty),
                                tussenvoegsel = (test.GetValue(3) != null ? test.GetValue(3).ToString() : string.Empty),
                                achternaam = (test.GetValue(4) != null ? test.GetValue(4).ToString() : string.Empty),
                                achtervoegsel = (test.GetValue(5) != null ? test.GetValue(1).ToString() : string.Empty),
                                geboortenaam = (test.GetValue(6) != null ? test.GetValue(2).ToString() : string.Empty),
                                geslacht = (test.GetValue(7) != null ? test.GetValue(3).ToString() : string.Empty),
                                status = (test.GetValue(8) != null ? test.GetValue(4).ToString() : string.Empty),
                                geboortedatum = (test.GetValue(9) != null ? test.GetValue(1).ToString() : string.Empty),
                                geboorteprecisie = (test.GetValue(10) != null ? test.GetValue(2).ToString() : string.Empty),
                                geboortedatum2 = (test.GetValue(11) != null ? test.GetValue(3).ToString() : string.Empty)
                            };
                        }
                        return personen;
                    }
                }
            }
        }
    }
}