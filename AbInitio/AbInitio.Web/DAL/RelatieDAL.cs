using AbInitio.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DAL
{
    public class RelatieDAL
    {

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
                        cmd.CommandText = "dbo.VerwijderRelatie";
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
    }
}