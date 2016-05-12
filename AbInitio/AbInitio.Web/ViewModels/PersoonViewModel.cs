using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.ViewModels
{
    public class PersoonViewModel
    {
        public persoon gebruiker { get; set; }
        public NieuwPersoonModel nieuwPersoonModel { get; set; }
        public List<personeninstamboom> StamboomGebruiker { get; set; }

        

    }
}