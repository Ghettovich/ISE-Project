using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.DbContexts
{
    public class PersoonPartial : persoon
    {

        public new string status { get; set; }
        public new string geboortedatum { get; set; }
        public new string geboortedatum2 { get; set; }


        public PersoonPartial()
        {
            
        }
    }
}