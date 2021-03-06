﻿using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbInitio.Web.ViewModels
{
    public class BeheerViewModel
    {
        public int stamboomid { get; set; }
        public PersoonPartial Persoon { get; set; }
        public PersoonPartial Vader { get; set; }
        public PersoonPartial Moeder { get; set; }
        public stamboom Stamboom { get; set; }
        public List<PersoonPartial> PersoonLijst { get; set; }
        public List<stamboom> StamboomLijst { get; set; }
        public AVRelatiePartial AvrRelatie { get; set; }





    }
}