using AbInitio.Web.App_Start;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DAL
{
    public class StamboomRechtenDAL
    {
        public IDataReader reader { get; set; }

        /// <summary>
        /// Hiermee worden alle accounts opgehaald die toegang hebben tot een stamboom.
        /// </summary>
        /// <param name="stamboomid"></param>
        /// <returns>Lijst met personen die toegang hebben tot een stamboom</returns>
        public List<AccountModel> getGerechtigden(int stamboomid)
        {
            List <AccountModel> accounts =  new List<AccountModel>();
            AccountModel account = null;
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_OverzichtRechten";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@stamboomid";
                        pm.Value = stamboomid;
                        cmd.Parameters.Add(pm);

                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            while (dr.Read())
                            {
                                account = new AccountModel
                                {
                                    stamboomAccountId = (int)dr["stamboomAccountid"],
                                    gebruikersnaam = dr["gebruikersnaam"].ToString(),
                                    stamboomRechten = (int)dr["stamboomrechten"]

                                };
                                accounts.Add(account);

                            }
                        }

                    }

                } return accounts;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Methode waarmee je kan zoeken op accounts die nog geen toegang
        /// hebben tot een bepaalde stamboom.
        /// </summary>
        /// <param name="gebruikersnaam"></param>
        /// <param name="stamboomId"></param>
        /// <returns>Lijst met accounts</returns>
        public List<AccountModel> getGebruikers(string gebruikersnaam, int stamboomId)
        {
            List <AccountModel> accounts =  new List<AccountModel>();
            AccountModel account = null;
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_AccountsZoeken";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@accountNaam";
                        pm.Value = gebruikersnaam;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@stamboomId";
                        pm.Value = stamboomId;
                        cmd.Parameters.Add(pm);

                        using (IDataReader dr = dbdc.CreateSqlReader())
                        {
                            while (dr.Read())
                            {
                                account = new AccountModel
                                {
                                    gebruikersnaam = dr["gebruikersnaam"].ToString(),
                                    accountId = (int)dr["accountid"]
                                };
                                accounts.Add(account);

                            }
                        }

                    }

                } return accounts;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Methode voor het toevoegen van een account in de stamboomToegang tabel.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="stamboomId"></param>
        /// <param name="rechten"></param>
        public void toevoegenGerechtigde(int accountId, int stamboomId, string rechten)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_ToevoegenRechten";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@stamboomid";
                        pm.Value = stamboomId;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@accountid";
                        pm.Value = accountId;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@recht";
                        pm.Value = Int32.Parse(rechten);
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
        /// Methode om een account te verwijderen uit de stamboomToegang tabel.
        /// </summary>
        /// <param name="stamboomAccountId"></param>
        public void verwijderenGerechtigde(int stamboomAccountId)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_VerwijderenRechten";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@stamboomAccountId";
                        pm.Value = stamboomAccountId;
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
        /// Methode om een account in de stamboomToegang te wijzigen van bewerken naar
        /// inzien en omgekeerd.
        /// </summary>
        /// <param name="stamboomAccountId"></param>
        /// <param name="rechten"></param>
        public void wijzigenGerechtigde(int stamboomAccountId, string rechten)
        {
            try
            {
                using (DataConfig dbdc = new DataConfig())
                {
                    dbdc.Open();
                    using (IDbCommand cmd = dbdc.CreateCommand())
                    {
                        cmd.CommandText = "dbo.spd_WijzigenRechten";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameter pm = cmd.CreateParameter();
                        pm.Direction = ParameterDirection.Input;

                        pm.ParameterName = "@stamboomAccountId";
                        pm.Value = stamboomAccountId;
                        cmd.Parameters.Add(pm);
                        pm = cmd.CreateParameter();
                        pm.ParameterName = "@recht";
                        pm.Value = rechten;
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
    }
}