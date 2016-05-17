using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Models
{
    public class RelatieModel
    {
        [Key]
        [Required]
        public int persoonid1 { get; set; }

        [Key]
        [Required]
        public int persoonid2 { get; set; }

        [Key]
        [Required]
        public int relatieid { get; set; }

        [Key]
        [Required]
        public int relatietypeid { get; set; }

        public PersoonPartial persoon1 { get; set; }
        public PersoonPartial persoon2 { get; set; }

        public List<PersoonPartial> PersoonLijst { get; set; }
        public List<SelectListItem> relatietypes { get; set; }
        public List<aanvullenderelatieinformatie> AvrLijst { get; set; }


    }
}