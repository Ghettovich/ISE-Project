﻿using AbInitio.Web.App_Start;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DAL
{
    public class StamboomDAL
    {
        public IDataReader reader { get; private set; }

        public void PersoonToevoegen(NieuwPersoonModel p)
        {
            try
            {
                using (AbInitioEntities abi = new AbInitioEntities())
                {
                    

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

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
        

        public static stamboom GetStamboom(int stamboomid)
        {
            stamboom stam = null;
            using (DataConfig dbdc = new DataConfig())
            {
                using (IDbCommand cmd = dbdc.CreateCommand())
                {
                    cmd.CommandText = "SELECT s.stamboomid, s.familienaam, s.levensverwachtingman, s.levensverwachtingvrouw, s.langstlevendeman, s.langstlevendevrouw, s.jongstlevendeman, s.jongstlevendevrouw, s.gemiddeldaantalkinderen, s.gemiddeldaantalgeboortes ";
                    cmd.CommandText += "FROM dbo.stamboom s ";
                    cmd.CommandText += "WHERE s.stamboomid = @stamboomid";

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
                                gemiddeldaantalgeboortes = (results.GetValue(9).ToString() != string.Empty ? (int)results.GetValue(9) : 0)
                            };
                        }
                    }
                }
            } return stam;
        }

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