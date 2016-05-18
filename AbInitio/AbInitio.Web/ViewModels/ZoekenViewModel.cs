using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.ViewModels
{
    public class ZoekenViewModel
    {
        public int stamboomId { get; set; }
        public PersoonPartial persoon { get; set; }
    }
}