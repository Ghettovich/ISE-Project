using AbInitio.Web.Code;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AbInitio.Web.ViewModels
{
    public class MatchViewModel
    {
        public List<PersoonPartial> PersonenInStamboom { get; set; }

        public List<MatchPersoon> MatchLijst { get; set; }

        public MatchingScore matching { get; set; }
        public PersoonPartial Persoon { get; set; }
        
        public bool FoundMatch { get; set; }

    }
}