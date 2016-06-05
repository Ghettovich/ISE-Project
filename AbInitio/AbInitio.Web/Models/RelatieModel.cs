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

        [Key]
        public int AvrID { get; set; }

        [Key]
        public int AvrInfoID { get; set; }

        [Key]
        public int relatietypeid { get; set; }
        public int kekuleid { get; set; }
        public int nieuwkekuleid { get; set; }
        public bool Vader { get; set; }
        public bool Moeder { get; set; }
        public string relatieinformatie { get; set; }
        public string Precisie { get; set; }
        [DataType(DataType.Date)]
        public string Van { get; set; }
        [DataType(DataType.Date)]
        public string Tot { get; set; }
        public DateTime VanDatum { get; set; }
        public DateTime TotDatum { get; set; }
        public PersoonPartial persoon1 { get; set; }
        public PersoonPartial persoon2 { get; set; }
        public RelatiePartial Relatie { get; set; }
        public AVRelatiePartial AvrRelatie { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime GewijzigdOp { get; set; }
        public List<AVRelatiePartial> AvrLijst { get; set; }
        public List<SelectListItem> Relatietypes { get; set; }
        public List<SelectListItem> DatumPrecisies { get; set; }
        public List<SelectListItem> AvrTypes { get; set; }
        public List<SelectListItem> Personen { get; set; }

    }    
}