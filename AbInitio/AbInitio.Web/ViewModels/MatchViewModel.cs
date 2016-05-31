using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.ViewModels
{
    public class MatchViewModel
    {
        public List<PersoonInStamboom> PersonenInStamboom { get; set; }

    }


    public class MatchingScore
    {

        PersoonPartial persoon = new PersoonPartial();
        List<PersoonPartial> list_personen = new List<PersoonPartial>();


    }
}