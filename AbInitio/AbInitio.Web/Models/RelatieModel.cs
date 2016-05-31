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

        public int StamboomdID { get; set; }
        public int relatietypeid { get; set; }
        public int AvRelatieID { get; set; }
        public string relatieinformatie { get; set; }
        public PersoonPartial persoon1 { get; set; }
        public PersoonPartial persoon2 { get; set; }
        public RelatiePartial Relatie { get; set; }
        public List<AVRelatiePartial> AvrLijst { get; set; }
        public List<SelectListItem> Relatietypes { get; set; }
        public List<SelectListItem> AvrTypes { get; set; }
        public List<SelectListItem> Personen { get; set; }
    }
}