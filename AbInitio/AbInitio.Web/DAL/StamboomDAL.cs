using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DAL
{
    public class StamboomDAL
    {

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

    }
}