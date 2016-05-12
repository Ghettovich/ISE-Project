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
    
    public class AccountDAL
    {
        public void RegistreerGebruuker(RegistreerModel model)
        {
            try
            {
                int accounttype = 1;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.RegistreerGebruiker";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@gebruikersnaam";
                        pm.Value = model.Gebruikersnaam;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@wachtwoord";
                        pm.Value = model.Wachtwoord;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@accounttype";
                        pm.Value = accounttype;
                        cmd.Parameters.Add(pm);
                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void Login(LoginModel model, out string error)
        {
            try
            {
                error = string.Empty;
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.UserLoginWeb";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@gebruikersnaam";
                        pm.Value = model.Gebruikersnaam;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@wachtwoord";
                        pm.Value = model.Wachtwoord;
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


        public static List<PersoonPartial> AllePersonen()
        {
            List<PersoonPartial> persoon_list = new List<PersoonPartial>();
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        int limit = 25;
                        int count = 0;
                        cmd.CommandText = "SELECT * FROM persoon";
                        dbdc.Open();
                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            while (dr.Read() && count < limit)
                            {
                                object[] test = new object[dr.FieldCount];
                                dr.GetValues(test);
                                persoon_list.Add(new PersoonPartial
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
                                    geboortedatum2 = (test.GetValue(11) != null ? test.GetValue(3).ToString() : string.Empty),
                                    geboorteplaats = (test.GetValue(12) != null ? test.GetValue(4).ToString() : string.Empty),
                                    adress = (test.GetValue(13) != null ? test.GetValue(1).ToString() : string.Empty),
                                    beroep = (test.GetValue(14) != null ? test.GetValue(2).ToString() : string.Empty)
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

    }
}